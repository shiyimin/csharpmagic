// 源码位置：第四章/traderbot-v2/Program.cs
// 编译与运行：dotnet run --option:ApiKey=1234 --option:ApiSecret=5678 --option:Site=huobi --option:Quote=USDT --option:Coin=BTC --option:Price 9500 --option:volumn 1
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using TraderBot.Core;
using TraderBot.Brokers;

namespace TraderBot
{
    class Program
    {
        delegate IBroker BrokerFactoryDelegate(string site);

        private static BrokerFactoryDelegate CreateBrokerFactoryAnonymous(IServiceProvider serviceProvider)
        {
            BrokerFactoryDelegate factory = delegate (string site)
            {
                if (string.Compare(site, "huobi", true) == 0)
                    return serviceProvider.GetService<Huobi>();
                else if (string.Compare(site, "hbm", true) == 0)
                    return serviceProvider.GetService<HuobiMargin>();
                else if (string.Compare(site, "okex", true) == 0)
                    return serviceProvider.GetService<Okex>();
                else
                    throw new InvalidOperationException($"不支持的站点：{site}");
            };
            return factory;
        }

        static void AddLoggingDetails(ILoggingBuilder builder)
        {
            var log = new LoggerConfiguration()
                        .MinimumLevel.Error()
                        .WriteTo.File($"tradebot.log", 
                                       rollingInterval: RollingInterval.Day)
                        .CreateLogger();

            builder.AddConsole();
            builder.AddSerilog(log);
        }

        static void Main(string[] args)
        {
            var switchMapping = new Dictionary<string, string>
            {
                {"-s", "Option:Site"}
            };

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables("TRADE_")
                .AddCommandLine(args, switchMapping)
                .Build();

            var option = new Option();
            config.GetSection("option").Bind(option);

            var container = new ServiceCollection()
                .AddLogging(AddLoggingDetails)
                .AddSingleton<Huobi>()
                .AddSingleton<HuobiMargin>()
                .AddSingleton<Okex>()
                .AddSingleton<ApiOption>(new ApiOption() { ApiKey = option.ApiKey, ApiSecret = option.ApiSecret })
                .AddTransient<BrokerFactoryDelegate>(CreateBrokerFactoryAnonymous)
                .AddSingleton<IConfiguration>(config)
                .BuildServiceProvider();

            using (container)
            {
                var factory = container.GetService<BrokerFactoryDelegate>();
                var broker = factory(option.Site);

                broker.Initialize(config["configdir"]);

                /*
                object oid = null;
                if (option.IsBuy)
                    oid = broker.BuyLimit(option.Quote, option.Coin, option.Volumn, option.Price);
                else
                    oid = broker.SellLimit(option.Quote, option.Coin, option.Volumn, option.Price);
                Console.WriteLine("下单结果：{0}", oid);
                */

                var orders = broker.GetOpenOrders(option.Quote, option.Coin);
                foreach (var order in orders) 
                    Console.WriteLine("未成交数量：{0}", order.QuantityRemaining);

                /* 
                var order = broker.GetOrderInfo(46540066103, option.Quote, option.Coin);
                Console.WriteLine("未成交数量：{0}", order.QuantityRemaining);

                var balances = broker.GetBalances();
                if (balances != null)
                {
                    foreach (var b in balances) {
                        if (b.Balance > 0) 
                            Console.WriteLine($"{b.Currency}, 总量：{b.Balance}，锁定：{b.Pending}");
                    }
                }
                */
            }

            /*
            var huobi = new Huobi("xxxx", "xxxx");
            huobi.Initialize(@"D:\workspace\writing\china-pub\C# Programming Magic\sample-code\第四章\traderbot-v2\Config\", null);
            var balances = huobi.GetBalances();
            Console.WriteLine($"{balances.Length}");
            */

            /*
            var huobi = new Huobi();
            var klines = huobi.GetKline("USDT", "BTC", DateTime.MinValue, DateTime.MaxValue, KlineInterval.D1);
            Console.WriteLine($"RESTful 返回长度：{klines.Length}, 第一个元素：{klines[0].CoinVolume}");
            */

            /*
            var klines = Huobi.RestfulClient.MarketHistoryKline("btcusdt", "1day", 20);
            Console.WriteLine($"RESTful 返回长度：{klines.Length}, 第一个元素：{klines[0].vol}");

            Huobi.UseWebSocket = true;
            while ( true )
            {
                klines = Huobi.RestfulClient.MarketHistoryKline("btcusdt", "1day", 20);
                if (klines != null && klines[0] != null)
                {
                    Console.WriteLine($"WebSocket 返回长度：{klines.Length}, 第一个元素：{klines[0].vol}");
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            */
        }
    }

    class Option
    {
        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }

        public string Site { get; set; }

        public string Quote { get; set; }

        public string Coin { get; set; }

        public bool IsBuy { get; set; }

        public decimal Price { get; set; }

        public decimal Volumn { get; set; }
    }
}
