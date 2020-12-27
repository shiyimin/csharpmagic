using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using TradeBot.Strategy.Band.Db;
using TraderBot.Test.Mock;
using TradeBot.Strategy.Band;
using Moq;
using TraderBot.Core;
using System.Linq.Expressions;

namespace TraderBot.Test
{
    [TestClass]
    public class 使用Moq测试
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
            var 期望买单Id = 100001;
            var site = "mock";

            using (var context = _container.GetService<BandTradeContext>())
            {
                context.BandStrategies.Add(new BandStrategy(userId: 1, quote: "USDT", coin: "BTC",
                    bidPrice: 2000, tradeVolume: 1, askProfit: null, askPrice: 8000));
                context.SaveChanges();
            }

            var mock = new Mock<IBroker>();
            mock.Setup(m => m.GetBalances()).Returns(new BalanceItem[]
            {
                new BalanceItem { Currency = "USDT", Available = 10000},
                new BalanceItem { Currency = "BTC", Available = 1},
            });
            mock.Setup(m => m.Support(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            mock.Setup(m => m.GetOrderBook("USDT", "BTC", 10)).Returns(
                开发数据库中测试.CreateOrderBook(site, "USDT", "BTC",
                    new decimal[,] { { 2000, 2 }, { 2001.88m, 1.2m } },
                    new decimal[,] { { 1999, 1 }, { 1998.99m, 2 } })
                );

            Expression<Func<IBroker, object>> buyExpression = m => m.BuyLimit("USDT", "BTC", It.IsAny<decimal>(), It.IsAny<decimal>());
            mock.Setup(buyExpression).Returns(期望买单Id);
            var engine = new BandStrategyEngine(_container);

            var broker = mock.Object;
            mock.Setup(m => m.CreateAuthenticatedInstance(It.IsAny<string>(), It.IsAny<string>())).Returns(broker);

            engine.Start("USDT", "BTC", broker);
            
            mock.Verify(buyExpression, Times.Once);

            // _db是之前创建的，需要重新查询一下数据库获取最新数据
            using (var context = _container.GetService<BandTradeContext>())
            {
                var bo = context.DbPlacedOrders.FirstOrDefault();
                Assert.IsNotNull(bo);
                Assert.AreEqual(期望买单Id.ToString(), bo.BidOrderId);
            }
        }
    }
}
