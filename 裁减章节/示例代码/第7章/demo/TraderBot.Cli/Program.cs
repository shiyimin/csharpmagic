using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using TradeBot.Strategy.Band.Db;
using TraderBot.Brokers;
using TraderBot.Core;

namespace TraderBot.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Usage();
                return;
            }

            var subCommandSwitch = new Dictionary<string, Dictionary<string, string>>()
            {
                { "trade", new Dictionary<string, string> {
                    {"-s", "site"},
                    {"-q", "quote"},
                    {"-c", "coin" },
                    {"-p", "price" },
                    {"-v", "volume" },
                    {"-d", "side" },
                }},
                { "account", new Dictionary<string, string> {
                    {"-s", "site"},
                    {"-b", "balance"},
                }},
                { "order", new Dictionary<string, string> {
                    {"-s", "site"},
                    {"-q", "quote"},
                    {"-c", "coin" },
                    {"-a", "active"},
                    {"-h", "history"},
                }},
                { "init", new Dictionary<string, string>() }
            };

            var subCmd = args[0].ToLower();
            if (!subCommandSwitch.ContainsKey(subCmd))
            {
                Usage();
                return;
            }

            var switchMapping = subCommandSwitch[subCmd];
            var builder = new ConfigurationBuilder()
                .AddCommandLine(args, switchMapping)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var services = new ServiceCollection()
                .AddLogging(config => config.AddConsole())
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<Huobi>()
                .AddSingleton<FakeBroker>()
                .AddTransient<BandTradeContext>()
                .AddTransient<BrokerFactoryDelegate>(serviceProvider => site =>
                {
                    if (string.Compare(site, "huobi", true) == 0)
                        return serviceProvider.GetService<Huobi>();
                    else if (string.Compare(site, "fake", true) == 0)
                        return serviceProvider.GetService<FakeBroker>();
                    else
                        throw new InvalidOperationException($"不支持的站点：{site}");
                })
                .BuildServiceProvider();

            using (services)
            {
                var logger = services.GetService<ILogger<Program>>();
                try
                {
                    switch (subCmd)
                    {
                        case "trade":
                            DoTrade(config, services, logger);
                            break;

                        case "init":
                            CreateDabase(config, services);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"执行过程中发生错误：{e.Message}");
                }
            }
        }

        private static void CreateDabase(IConfigurationRoot config, ServiceProvider services)
        {
            using (var db = services.GetService<BandTradeContext>())
            {
                db.Database.EnsureCreated();
            }
        }

        static void Usage()
        {
            Console.WriteLine("帮助文档，使用方法请阅读源码！");
        }

        delegate IBroker BrokerFactoryDelegate(string site);

        private static void DoTrade(IConfigurationRoot config, ServiceProvider services, ILogger logger)
        {
            var factory = services.GetService<BrokerFactoryDelegate>();
            var broker = factory(config["site"]);
            broker.Initialize(Directory.GetCurrentDirectory());

            var price = decimal.Parse(config["price"]);
            var volume = decimal.Parse(config["volume"]);
            var buy = string.Compare(config["side"], "buy", true) == 0;

            object oid;
            if (buy)
                oid = broker.BuyLimit(config["quote"], config["coin"], volume, price);
            else
                oid = broker.SellLimit(config["quote"], config["coin"], volume, price);

            if (oid != null)
            {
                logger.LogInformation($"成功下达订单: {oid}");
                using (var db = services.GetService<BandTradeContext>())
                {
                    var order = new DbPlacedOrder()
                    {
                        BrokerSite = config["site"]
                    };

                    if (buy)
                    {
                        order.BidQuote = config["quote"];
                        order.BidPrice = price;
                        order.BidOrderId = oid.ToString();
                        order.BidTimestamp = DateTime.Now;
                    }
                    else
                    {
                        order.AskQuote = config["quote"];
                        order.AskPrice = price;
                        order.AskOrderId = oid.ToString();
                        order.AskTimestamp = DateTime.Now;
                    }
                    order.Coin = config["coin"];
                    order.TradingVolume = volume;

                    db.DbPlacedOrders.Add(order);
                    db.SaveChanges();
                }
            }
            else
            {
                logger.LogWarning($"下单失败！");
            }
        }
    }
}
