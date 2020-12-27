using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TradeBot.Strategy.Band;
using TradeBot.Strategy.Band.Db;
using TraderBot.Brokers;
using TraderBot.Core;
using TraderBot.Test.Mock;

namespace TraderBot.Test
{
    [TestClass]
    public class 开发数据库中测试
    {
        private BandTradeContext _db;
        private static IServiceProvider _container;

        [ClassInitialize]
        public static void 准备依赖注入环境(TestContext tc)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _container = new ServiceCollection()
                .AddLogging(config => config.AddDebug())
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<Huobi>()
                .AddTransient<BandTradeContext>()
                .BuildServiceProvider();
        }

        [TestInitialize]
        public void 重新构建数据库()
        {
            var factory = new DesignTimeBandTradeContextFactory();
            _db = factory.CreateDbContext(null);
            Assert.IsTrue(_db.Database.EnsureCreated());

            _db.UserTradeApiSettings.Add(new UserTradeApiSetting("1", "1-secrete"));
            _db.SaveChanges();
        }

        [TestCleanup]
        public void 删除数据库()
        {
            if (_db != null)
            {
                Assert.IsTrue(_db.Database.EnsureDeleted());
                _db.Dispose();
                _db = null;
            }
        }

        [TestMethod]
        [TestCategory("回归测试")]
        public void 验证使用实际数据库根据策略下达买单()
        {
            var quote = "USDT";
            var coin = "BTC";
            var 期望买单Id = 100001;
            var site = "huobi";
            var uid = 1;

            _db.BandStrategies.Add(new BandStrategy(userId: uid, quote: quote, coin: coin,
                bidPrice: 2000, tradeVolume: 1, askProfit: null, askPrice: 8000));
            _db.SaveChanges();

            var broker = new FakeBroker(site, new Dictionary<string, decimal>()
            {
                { "USDT", 10000 },
                { "BTC", 1 }
            });
            var engine = new BandStrategyEngine(_container);

            broker.FakedOrderBook = (b, q, c) =>
            {
                var ob = CreateOrderBook(site, q, c,
                    new decimal[,] { { 2000, 2 }, { 2001.88m, 1.2m } },
                    new decimal[,] { { 1999, 1 }, { 1998.99m, 2 } });

                return ob;
            };
            broker.FakedBuyLimit = (b, q, c, v, p) =>
            {
                return 期望买单Id;
            };
            engine.Start(quote, coin, broker);
            Assert.AreEqual(1, broker.BuyLimitCallCount);
            Assert.AreEqual(0, broker.SellLimitCallCount);

            // _db是之前创建的，需要重新查询一下数据库获取最新数据
            using (var context = _container.GetService<BandTradeContext>())
            {
                var bo = context.DbPlacedOrders.FirstOrDefault();
                Assert.IsNotNull(bo);
                Assert.AreEqual(期望买单Id.ToString(), bo.BidOrderId);
            }

            var openOrders = broker.GetOpenOrders(quote, coin);
            Assert.AreEqual(1, openOrders.Length);
            Assert.IsNotNull(openOrders.SingleOrDefault(o => (int)o.Id == 期望买单Id));
        }

        [TestMethod]
        public void 验证使用实际数据库高抛低吸策略()
        {
            var quote = "USDT";
            var coin = "BTC";
            var 期望卖单Id = 100001;
            var site = "huobi";
            var uid = 1;

            _db.BandStrategies.Add(new BandStrategy(uid, quote, coin, 2000, 1, null, 8000));

            var 模拟已有下单 = new DbPlacedOrder()
            {
                UserId = uid,
                BidQuote = quote,
                Coin = coin,
                BrokerSite = site,
                BidOrderId = Guid.NewGuid().ToString(),
                BidPrice = 2000,
                StrategyId = 1,
                TradingVolume = 1,
                BidTimestamp = DateTime.Now
            };
            _db.DbPlacedOrders.Add(模拟已有下单);
            _db.SaveChanges();
            var 模拟已有下单Id = 模拟已有下单.Id;

            var broker = new FakeBroker(site, new Dictionary<string, decimal>()
            {
                { "USDT", 0 },
                { "BTC", 1 }
            });
            var engine = new BandStrategyEngine(_container);

            broker.FakedOrderBook = (b, q, c) =>
            {
                var ob = CreateOrderBook(site, q, c,
                    new decimal[,] { { 8001, 2 }, { 8001.88m, 1.2m } },
                    new decimal[,] { { 8000, 1 }, { 7999.99m, 2 } });

                return ob;
            };
            broker.FakedSellLimit = (b, q, c, v, p) =>
            {
                return 期望卖单Id;
            };
            engine.Start(quote, coin, new IBroker[] { broker });
            Assert.AreEqual(0, broker.BuyLimitCallCount);
            Assert.AreEqual(1, broker.SellLimitCallCount);

            // _db是之前创建的，需要重新查询一下数据库获取最新数据
            using (var context = _container.GetService<BandTradeContext>())
            {
                模拟已有下单 = context.DbPlacedOrders.Single(o => o.Id == 模拟已有下单Id);
                Assert.AreEqual(期望卖单Id.ToString(), 模拟已有下单.AskOrderId);
                Assert.AreEqual(8000, 模拟已有下单.AskPrice);
            }

            var openOrders = broker.GetOpenOrders(quote, coin);
            Assert.AreEqual(1, openOrders.Length);
            Assert.IsNotNull(openOrders.SingleOrDefault(o => (int)o.Id == 期望卖单Id));
        }

        public static OrderBook CreateOrderBook(string sitename, string quote, string coin, decimal[,] asks, decimal[,] bids)
        {
            var ob = new OrderBook()
            {
                Site = sitename,
                Quote = quote,
                Coin = coin,
                Timestamp = DateTime.Now,
                LowestAsk = asks[0, 0],
                LowestAskVolume = asks[0, 1],
                HighestBid = bids[0, 0],
                HighestBidVolume = bids[0, 1]
            };

            ob.SetAsks(asks);
            ob.SetBids(bids);

            return ob;
        }
    }
}
