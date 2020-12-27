using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using TradeBot.Strategy.Band.Db;
using TraderBot.Core;

namespace TradeBot.Strategy.Band
{
    public class BandStrategyEngine
    {
        private IServiceProvider _container;
        private ILogger<BandStrategyEngine> _logger;

        public BandStrategyEngine(IServiceProvider container)
        {
            _container = container;
            _logger = container.GetService<ILogger<BandStrategyEngine>>();
        }

        public void Start(string quote, string coin, params IBroker[] brokers)
        {
            BandStrategy[] cachedStrategis = null;
            using (var context = _container.GetService<BandTradeContext>())
                cachedStrategis = context.BandStrategies.Where(s => s.Quote == quote && s.Coin == coin).ToArray();

            foreach (var strategy in cachedStrategis)
            {
                foreach (var broker in brokers)
                {
                    try
                    {
                        if (broker.Support(quote, coin))
                            ProcessStrategy(strategy, broker);
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning($"在针对交易所【{broker.Name}】处理策略Id【{strategy.Id}】时发生异常，异常消息：【{e.Message}】，堆栈：【{e.StackTrace}】");
                    }
                }
            }
        }

        private void ProcessStrategy(BandStrategy strategy, IBroker broker)
        {
            // 如果数据库中有脏数据，跳过处理该策略
            if (!strategy.AskPrice.HasValue && !strategy.AskProfit.HasValue) return;

            using (var context = _container.GetService<BandTradeContext>())
            {
                // 要查询用户余额，挂单等信息，需要授权的接口权限
                var authinfo = context.UserTradeApiSettings.SingleOrDefault(u => u.Id == strategy.UserId);
                if (authinfo == null) return;

                var authBroker = broker.CreateAuthenticatedInstance(authinfo.ApiKey, authinfo.ApiSecret);

                // 查看该账户下面是否有该策略的买单，没有的话在价格合适的时候下买单
                var pendingBids = context.DbPlacedOrders.Where(
                    o => o.StrategyId == strategy.Id && o.UserId == strategy.UserId && !o.Closed).ToArray();

                // 如果策略没有买单而且价格合适，就下达买单
                if (pendingBids.Length == 0)
                    PlaceBidOrder(strategy, authBroker, context);
                else // 如果有未成交的买单且价格合适，就下达相应的卖单获取盈利
                    PlaceAskOrder(strategy, authBroker, context, pendingBids);

                // 保存结果
                context.SaveChanges();
            }
        }

        private void PlaceAskOrder(BandStrategy strategy, IBroker authBroker, BandTradeContext context, DbPlacedOrder[] pendingBids)
        {
            // 如果网络异常无法获取订单表信息，跳过处理该策略
            var orderbook = authBroker.GetOrderBook(strategy.Quote, strategy.Coin);
            if (orderbook == null) return;

            if (pendingBids.Length > 1)
                _logger.LogWarning($"策略{strategy.Id}在数据库中有一笔以上的未完成买单！");

            foreach (var pendingBid in pendingBids)
            {
                // 判断用户账户的余额是否足够下卖单
                var askPrice = strategy.AskPrice;
                if (!askPrice.HasValue || askPrice.Value == 0)
                    askPrice = strategy.BidPrice * (1 + strategy.AskProfit.Value);

                if (askPrice.Value == 0) return;

                // 如果当前的买价不满足预设条件，跳过处理该策略
                if (orderbook.HighestBid < askPrice) return;

                // 判断用户账户的余额是否足够下卖单
                var balances = authBroker.GetBalances();
                if (balances == null || balances.Length == 0) return;

                // 由于是下卖单，所以只要判断卖出的币余额是否足够即可
                var balance = balances.SingleOrDefault(b => b.Currency == strategy.Coin);
                if (balance == null) return;

                var volume = pendingBid.TradingVolume;
                if (balance.Available < volume) return;
                // 下达卖单
                var oid = authBroker.SellLimit(strategy.Quote, strategy.Coin, volume, askPrice.Value);
                if (oid != null)
                {
                    _logger.LogInformation($"在【{authBroker.Name}】下达【{strategy.Coin}/{strategy.Quote}】卖单，价格：{askPrice}，交易量：{strategy.TradeVolume}，策略Id:{strategy.Id}，策略单Id：{pendingBid.Id}");
                    pendingBid.AskOrderId = oid.ToString();
                    pendingBid.AskQuote = strategy.Quote;
                    pendingBid.AskPrice = askPrice.Value;
                }
            }
        }

        private void PlaceBidOrder(BandStrategy strategy, IBroker authBroker, BandTradeContext context)
        {
            // 如果网络异常无法获取订单表信息，跳过处理该策略
            var orderbook = authBroker.GetOrderBook(strategy.Quote, strategy.Coin);
            if (orderbook == null) return;

            // 最新卖价已经低到合适的位置，则下达买单
            if (strategy.BidPrice > orderbook.LowestAsk) return;

            // 判断用户账户的余额是否足够下买单
            var balances = authBroker.GetBalances();
            if (balances == null || balances.Length == 0) return;

            // 买单需要判断计价币是否有足够余额
            var balance = balances.SingleOrDefault(b => b.Currency == strategy.Quote);
            if (balance == null || balance.Available == 0) return;

            if (balance.Available < (strategy.TradeVolume * strategy.BidPrice * (1 + authBroker.Fee))) return;

            // 余额足够，下达买单
            var bid = authBroker.BuyLimit(strategy.Quote, strategy.Coin, strategy.TradeVolume, strategy.BidPrice);
            if (bid == null) return;

            _logger.LogInformation($"在【{authBroker.Name}】下达【{strategy.Coin}/{strategy.Quote}】买单，价格：{strategy.BidPrice}，交易量：{strategy.TradeVolume}，策略Id:{strategy.Id}");
            // 将最新的买单记录到数据库中
            context.DbPlacedOrders.Add(new DbPlacedOrder()
            {
                BidOrderId = bid.ToString(),
                BidPrice = strategy.BidPrice,
                BidQuote = strategy.Quote,
                Coin = strategy.Coin,
                BidTimestamp = DateTime.Now,
                BrokerSite = authBroker.Name,
                StrategyId = strategy.Id,
                TradingVolume = strategy.TradeVolume,
                UserId = strategy.UserId
            });
        }
    }
}
