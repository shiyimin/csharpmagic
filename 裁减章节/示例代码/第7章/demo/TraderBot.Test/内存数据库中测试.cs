using Microsoft.EntityFrameworkCore;
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
    public class 内存数据库中测试
    {
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
                .AddTransient<BandTradeContext>(sp =>
                {
                    var options = new DbContextOptionsBuilder<BandTradeContext>()
                        .UseInMemoryDatabase(databaseName: "bandmemorydb")
                        .Options;

                    return new BandTradeContext(options, sp.GetService<IConfiguration>(), sp.GetService<ILoggerFactory>());
                })
                .BuildServiceProvider();
        }

        [TestInitialize]
        public void 准备测试数据()
        {
            using (var context = _container.GetService<BandTradeContext>())
            {
                var key = "1";
                if (!context.UserTradeApiSettings.Any(u => u.ApiKey == key))
                {
                    context.UserTradeApiSettings.Add(new UserTradeApiSetting(key, "1-secrete"));
                    context.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void 验证使用实际数据库根据策略下达买单()
        {
            var quote = "USDT";
            var coin = "BTC";
            var 期望买单Id = 100001;
            var site = "huobi";
            var uid = 1;

            using (var context = _container.GetService<BandTradeContext>())
            {
                context.BandStrategies.Add(new BandStrategy(userId: uid, quote: quote, coin: coin,
                    bidPrice: 2000, tradeVolume: 1, askProfit: null, askPrice: 8000));
                context.SaveChanges();
            }

            var broker = new FakeBroker(site, new Dictionary<string, decimal>()
            {
                { "USDT", 10000 },
                { "BTC", 1 }
            });
            var engine = new BandStrategyEngine(_container);

            broker.FakedOrderBook = (b, q, c) =>
            {
                var ob = 开发数据库中测试.CreateOrderBook(site, q, c,
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
    }
}
