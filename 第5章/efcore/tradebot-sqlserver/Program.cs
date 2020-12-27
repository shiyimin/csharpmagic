using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace tradebot_v3
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args)
                .Build();

            var container = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddTransient<TradeHistoryContext>()
                .BuildServiceProvider();

            using (container)
            {
                int orderId = 0;
                using (var context = container.GetService<TradeHistoryContext>())
                {
                    var order = new DbPlacedOrder("BTC", "huobi", "order-123", "USDT") {
                        BidPrice = 8100.988m, TradingVolume = 1, BidTimestamp = DateTime.Now
                    };
                    context.DbPlacedOrders.Add(order);
                    context.SaveChanges();
                    orderId = order.Id;
                    Console.WriteLine("order id: {0}", orderId);
                }

                using (var context = container.GetService<TradeHistoryContext>())
                {
                    var order = context.DbPlacedOrders.Single(o => o.Id == orderId);
                    Console.WriteLine($"{order.Coin}/{order.BidQuote}：{order.BidPrice}");
                    order.BidQuote = "USD";
                    context.SaveChanges();
                }
                
                var orderModify = new DbPlacedOrder() { Id = orderId };
                using (var context = container.GetService<TradeHistoryContext>())
                {
                    context.DbPlacedOrders.Attach(orderModify);
                    orderModify.BidPrice = 8888.988m;
                    context.SaveChanges();
                }

                using (var context = container.GetService<TradeHistoryContext>())
                {
                    var order = context.DbPlacedOrders.Single(o => o.Id == orderId);
                    Console.WriteLine($"{order.Coin}/{order.BidQuote}：{order.BidPrice}");
                }
            }
        }
    }
}
