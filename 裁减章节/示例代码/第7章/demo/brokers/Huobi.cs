using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TraderBot.Core;

namespace TraderBot.Brokers
{
    public partial class Huobi : IBroker
    {
        private const string SITE_NAME = "Huobi";
        private string _apiKey;
        private string _apiSecret;
        private ILogger<Huobi> _logger;

        public string Name { get; private set; } = SITE_NAME;

        public decimal Fee => 0.002m;

        public Huobi(ILogger<Huobi> logger)
        {
            _logger = logger;
        }

        private Huobi(string apiKey, string apiSecret, ILogger<Huobi> logger) : this(logger)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public static bool UseWebSocket
        {
            get; set;
        }

        private static string[] s_supportedPairs;
        public bool Support(string quote, string coin)
        {
            var pair = BuildSymbol(quote, coin);
            return Array.BinarySearch(s_supportedPairs, pair) >= 0;
        }

        public bool CancelOrder(object orderid, string quote, string coin)
        {
            var symbol = BuildSymbol(quote, coin);
            var response = RestfulClient.OrdersSubmitCancel(orderid.ToString(), _apiKey, _apiSecret);
            return !string.IsNullOrEmpty(response);
        }

        public Ticker[] GetKline(string quote, string coin, DateTime begin, DateTime end, KlineInterval interval)
        {
            MarketHistoryKlineResponse[] response = null;
            var symbol = BuildSymbol(quote, coin);
            var period = "1day";
            var size = 100;

            switch (interval)
            {
                case KlineInterval.M1:
                    period = "1min";
                    break;
                case KlineInterval.M5:
                    period = "5min";
                    break;
                case KlineInterval.M15:
                    period = "15min";
                    break;
                case KlineInterval.M30:
                    period = "30min";
                    break;
                case KlineInterval.H1:
                    period = "60min";
                    break;
                case KlineInterval.D1:
                    period = "1day";
                    break;
                case KlineInterval.W1:
                    period = "1week";
                    break;
                default:
                    break;
            }

            if (UseWebSocket)
            {
                if (!s_WebSocketInitialized)
                {
                    s_WebSocketInitialized = WebSocketClient.Init();
                    response = null;
                }
                else
                {
                    response = WebSocketClient.MarketHistoryKline(symbol, period, size);
                }
            }
            else
            {
                response = RestfulClient.MarketHistoryKline(symbol, period, size);
            }

            if (response != null)
            {
                var result = new List<Ticker>();
                foreach (var item in response)
                {
                    var ticker = new Ticker()
                    {
                        Site = Name,
                        Quote = quote,
                        Coin = coin,
                        Open = item.open,
                        High = item.high,
                        Low = item.low,
                        Close = item.close,
                        QuoteVolume = item.vol,
                        Timestamp = DateTime.Now
                    };
                    result.Add(ticker);
                }

                return result.ToArray();
            }
            else
            {
                return null;
            }
        }

        public OrderBook GetOrderBook(string quote, string coin, int limit = OrderBook.ORDER_CACHING_COUNT)
        {
            if (!Support(quote, coin)) return null;

            var response = RestfulClient.MarketDepth(BuildSymbol(quote, coin), limit);
            return DepthToOrder(response, quote, coin, limit);
        }

        public virtual object BuyLimit(string quote, string coin, decimal quantity, decimal price)
        {
            return PlaceOrder(quote, coin, quantity, price, "buy-limit", "api");
        }

        public virtual object SellLimit(string quote, string coin, decimal quantity, decimal price)
        {
            return PlaceOrder(quote, coin, quantity, price, "sell-limit", "api");
        }

        protected object PlaceOrder(string quote, string coin, decimal quantity, decimal price, string direction, string source)
        {
            var symbol = BuildSymbol(quote, coin);
            _logger.LogWarning($"{source}下达交易对 {direction} {symbol}，价格：{price} {quote}, 数量：{quantity}");
            return RestfulClient.OrderPlace(symbol, direction, quantity.ToString(), price.ToString(), 获取交易账号(quote, coin), _apiKey, _apiSecret, source);
        }

        private string _现货交易账号;
        protected virtual string 获取交易账号(string quote = null, string coin = null)
        {
            if (string.IsNullOrEmpty(_现货交易账号) && (AllAccounts != null && AllAccounts.Length > 0))
            {
                foreach (var account in AllAccounts)
                {
                    if (string.Compare(account.type, "spot", true) == 0)
                    {
                        _现货交易账号 = account.id.ToString();
                        break;
                    }
                }
            }

            return _现货交易账号;
        }

        public ExchangeOrder GetOrderInfo(object orderid, string quote, string coin)
        {
            var response = RestfulClient.OrderDetails(orderid.ToString(), _apiKey, _apiSecret);
            if (response != null)
            {
                var ret = new ExchangeOrder()
                {
                    Quote = quote,
                    Coin = coin,
                    Site = SITE_NAME,
                    IsCancelled = string.Compare(response.state, "cancelling", true) == 0,
                    Price = decimal.Parse(response.price),
                    Quantity = decimal.Parse(response.amount),
                    Side = string.Compare(response.type, "buy-limit", true) == 0 ? TradeSide.Buy : TradeSide.Sell
                };

                ret.QuantityRemaining = ret.Quantity - decimal.Parse(response.filled_amount);
                return ret;
            }
            else
            {
                return null;
            }
        }

        public virtual BalanceItem[] GetBalances()
        {
            return GetBalancesImpl(获取交易账号());
        }

        protected BalanceItem[] GetBalancesImpl(string accountid)
        {
            try
            {
                var ab = RestfulClient.AccountsBalance(accountid, _apiKey, _apiSecret);
                var ret = new List<BalanceItem>();
                foreach (var item in ab.list)
                {
                    var balance = new BalanceItem()
                    {
                        Currency = item.currency,
                        Balance = decimal.Parse(item.balance)
                    };

                    if (string.Compare(item.type, "trade", true) == 0)
                        balance.Available = decimal.Parse(item.balance);

                    if (string.Compare(item.type, "frozen", true) == 0)
                        balance.Pending = decimal.Parse(item.balance);

                    ret.Add(balance);
                }

                return ret.ToArray();
            }
            catch (Exception e)
            {
                _logger.LogWarning($"[GetBalancesImpl] 获取账户余额发生错误，错误详情：{e.Message},堆栈信息：\n{e.StackTrace}");
                return null;
            }
        }

        public ExchangeOrder[] GetOpenOrders(string quote, string coin)
        {
            var symbol = BuildSymbol(quote, coin);
            var oo = RestfulClient.OpenOrders(获取交易账号(quote, coin), symbol, null, _apiKey, _apiSecret, 100);
            if (oo != null)
            {
                var ret = new List<ExchangeOrder>();
                foreach (var x in oo)
                {
                    var eoi = new ExchangeOrder()
                    {
                        Id = x.id,
                        Quote = quote,
                        Coin = coin,
                        IsCancelled = string.Compare(x.state, "cancelling", true) == 0,
                        Price = decimal.Parse(x.price),
                        Quantity = decimal.Parse(x.amount),
                        Side = string.Compare(x.type, "buy-limit", true) == 0 ? TradeSide.Buy : TradeSide.Sell,
                        Site = SITE_NAME,
                        PlacedTimestamp = FromUnixTimestamp(x.created_at)
                    };

                    eoi.QuantityRemaining = eoi.Quantity - decimal.Parse(x.filled_amount);
                    ret.Add(eoi);
                }

                return ret.ToArray();
            }
            else
            {
                return null;
            }
        }

        public TradingLimitInfo[] GetTradeLimit(string quote, string coin)
        {
            return new TradingLimitInfo[] { };
        }

        protected AccountsResponse[] AllAccounts { get; private set; }

        private static bool s_initialized;

        public void Initialize(string confdir)
        {
            if (!s_initialized)
            {
                var path = Path.Combine(confdir, "huobi-symbols.csv");
                if (!System.IO.File.Exists(path))
                    throw new InvalidOperationException(string.Format("找不到symbols配置文件：{0}", path));

                var text = System.IO.File.ReadAllText(path);
                s_supportedPairs = File.ReadAllLines(path);
                Array.Sort(s_supportedPairs);

                if (UseWebSocket)
                    s_initialized = WebSocketClient.Init();
                else
                    s_initialized = true;
            }

            if (!s_initialized)
                throw new InvalidOperationException("火币API没有被正确初始化！");

            AllAccounts = RestfulClient.GetAllAccount(_apiKey, _apiSecret);
        }

        public void Cleanup() { /* 没有清理工作 */ }

        public class MarketHistoryKlineResponse
        {
            public long id { get; set; }

            public decimal amount { get; set; }

            public int count { get; set; }

            public decimal open { get; set; }

            public decimal close { get; set; }

            public decimal low { get; set; }

            public decimal high { get; set; }

            public decimal vol { get; set; }
        }

        public class MarketTickersResponse
        {
            public decimal amount { get; set; }

            public int count { get; set; }

            public decimal open { get; set; }

            public decimal close { get; set; }

            public decimal low { get; set; }

            public decimal high { get; set; }

            public decimal vol { get; set; }

            public string symbol { get; set; }
        }

        public class MarketDepthResponse
        {
            public long ts { get; set; }

            public long version { get; set; }

            public decimal[][] bids { get; set; }

            public decimal[][] asks { get; set; }
        }

        public class AccountsResponse
        {
            public long id { get; set; }

            public string state { get; set; }

            public string type { get; set; }

            public string subtype { get; set; }

            [JsonProperty(PropertyName = "user-id")]
            public long user_id { get; set; }
        }

        public class AccountsBalanceResponse
        {
            public long id { get; set; }

            public string state { get; set; }

            public string type { get; set; }

            public ListType[] list { get; set; }

            public class ListType
            {
                public string balance { get; set; }

                public string currency { get; set; }

                public string type { get; set; }
            }
        }

        public class OpenOrdersResponse
        {
            public long id { get; set; }

            [JsonProperty(PropertyName = "account-id")]
            public long account_id { get; set; }

            public string symbol { get; set; }

            public string price { get; set; }

            public string amount { get; set; }

            [JsonProperty(PropertyName = "created-at")]
            public long created_at { get; set; }

            public string type { get; set; }

            [JsonProperty(PropertyName = "filled-amount")]
            public string filled_amount { get; set; }

            [JsonProperty(PropertyName = "filled-cash-amount")]
            public string filled_cash_amount { get; set; }

            [JsonProperty(PropertyName = "filled-fees")]
            public string filled_fees { get; set; }

            public string source { get; set; }

            public string state { get; set; }
        }

        public class OrderDetailResponse
        {
            [JsonProperty(PropertyName = "account-id")]
            public long account_id { get; set; }

            public string amount { get; set; }

            [JsonProperty(PropertyName = "canceled-at")]
            public long canceled_at { get; set; }

            [JsonProperty(PropertyName = "created-at")]
            public long created_at { get; set; }

            [JsonProperty(PropertyName = "field-amount")]
            public string filled_amount { get; set; }

            [JsonProperty(PropertyName = "field-cash-amount")]
            public string filled_cash_amount { get; set; }

            [JsonProperty(PropertyName = "field-fees")]
            public string filld_fees { get; set; }

            [JsonProperty(PropertyName = "finished-at")]
            public string finished_at { get; set; }

            public long id { get; set; }

            public string price { get; set; }

            public string source { get; set; }

            public string state { get; set; }

            public string symbol { get; set; }

            public string type { get; set; }
        }

        protected static string BuildSymbol(string quote, string coin)
        {
            return string.Format($"{coin}{quote}").ToLower();
        }

        private static T FromJson<T>(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var item = JsonConvert.DeserializeObject<T>(json);
                return item;
            }
            else
            {
                return default(T);
            }
        }

        private static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static DateTime FromUnixTimestamp(long value)
        {
            // 参考文档：https://stackoverflow.com/a/26225951
            return DateTimeOffset.FromUnixTimeMilliseconds(value).UtcDateTime;
        }

        public static DateTime FromUnixTimestamp(double value)
        {
            var date = new DateTime(1970, 1, 1);
            var utc = date.AddSeconds(value);
            //var test = utc.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return utc.ToLocalTime();
        }

        private static OrderBook DepthToOrder(MarketDepthResponse depth, string quote, string coin, int limit = OrderBook.ORDER_CACHING_COUNT)
        {
            var asks = depth.asks;
            var bids = depth.bids;
            var obi = new OrderBook(limit)
            {
                Site = SITE_NAME,
                Timestamp = DateTime.Now,
                Coin = coin.ToUpper(),
                Quote = quote.ToUpper(),
                HighestBid = bids[0][0],
                HighestBidVolume = bids[0][1],
                LowestAsk = asks[limit - 1][0],
                LowestAskVolume = asks[limit - 1][1]
            };

            for (var i = 0; i < limit; ++i)
            {
                obi.Asks[i, 1] = asks[i][0];
                obi.Asks[i, 2] = asks[i][1];
                obi.Bids[i, 1] = bids[i][0];
                obi.Bids[i, 2] = bids[i][1];
            }

            return obi;
        }

        public IBroker CreateAuthenticatedInstance(string apiKey, string apiSecret)
        {
            return new Huobi(apiKey, apiSecret, _logger);
        }
    }
}
