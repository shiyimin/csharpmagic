using System;
using System.Collections.Generic;
using System.Linq;
using TraderBot.Core;

namespace TraderBot.Test.Mock
{
    public class FakeBroker : IBroker
    {
        private Dictionary<string, decimal> _balances;
        private List<ExchangeOrder> _orders = new List<ExchangeOrder>();

        public FakeBroker(string name) { _name = name; }

        public FakeBroker(string name, Dictionary<string, decimal> balances) : this(name)
        {
            _balances = balances;
        }

        private string _name;
        public string Name => _name;

        public decimal Fee => 0m;

        public int BuyLimitCallCount { get; private set; }
        public Func<FakeBroker, string, string, decimal, decimal, int> FakedBuyLimit { get; set; }

        public int SellLimitCallCount { get; private set; }
        public Func<FakeBroker, string, string, decimal, decimal, int> FakedSellLimit { get; set; }

        public object BuyLimit(string quote, string coin, decimal quantity, decimal price)
        {
            if (FakedBuyLimit == null) return 0;

            BuyLimitCallCount++;
            var ret = FakedBuyLimit(this, quote, coin, quantity, price);
            if (ret > 0)
            {
                _orders.Add(new ExchangeOrder()
                {
                    Id = ret,
                    Side = TradeSide.Buy,
                    Site = Name,
                    Quote = quote,
                    Coin = coin,
                    PlacedTimestamp = DateTime.Now,
                    Price = price,
                    Quantity = quantity,
                    QuantityRemaining = quantity
                });

                return ret;
            }
            else
            {
                return 0;
            }
        }

        public object SellLimit(string quote, string coin, decimal quantity, decimal price)
        {
            if (FakedSellLimit == null) return 0;

            SellLimitCallCount++;
            var ret = FakedSellLimit(this, quote, coin, quantity, price);
            if (ret > 0)
            {
                _orders.Add(new ExchangeOrder()
                {
                    Id = ret,
                    Side = TradeSide.Buy,
                    Site = Name,
                    Quote = quote,
                    Coin = coin,
                    PlacedTimestamp = DateTime.Now,
                    Price = price,
                    Quantity = quantity,
                    QuantityRemaining = quantity
                });
                return ret;
            }
            else
            {
                return 0;
            }
        }

        public bool CancelOrder(object orderid, string quote, string coin)
            => throw new NotImplementedException();

        public void Cleanup() { }

        public BalanceItem[] GetBalances()
        {
            return _balances.Select(b => new BalanceItem()
            {
                Currency = b.Key,
                Available = b.Value
            }).ToArray();
        }

        public Ticker[] GetKline(
            string quote, string coin, DateTime begin, DateTime end, KlineInterval interval)
            => throw new NotImplementedException();

        public ExchangeOrder[] GetOpenOrders(string quote, string coin)
        {
            return _orders.ToArray();
        }

        public Func<FakeBroker, string, string, OrderBook> FakedOrderBook { get; set; }

        public OrderBook GetOrderBook(string quote, string coin, int limit = 10)
        {
            var ret = FakedOrderBook(this, quote, coin);
            return ret;
        }

        public ExchangeOrder GetOrderInfo(
            object orderid, string quote, string coin) => throw new NotImplementedException();


        public TradingLimitInfo[] GetTradeLimit(string quote, string coin) => null;

        public void Initialize(string confdir) { }

        public bool Support(string quote, string coin) => true;

        public IBroker CreateAuthenticatedInstance(string apiKey, string apiSecret) => this;
    }
}
