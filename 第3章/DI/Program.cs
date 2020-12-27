using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DI
{
    class Program
    {
        delegate IBroker BrokerFactoryDelegate(string site);

        private static BrokerFactoryDelegate CreateBrokerFactoryCaptured(IServiceProvider serviceProvider)
        {
            var wrapper = new BrokerFactoryDelegateWithCapturedVariable() { serviceProvider = serviceProvider };
            return wrapper.BrokerFactoryDelegateImpl;
        }
        
        private static BrokerFactoryDelegate CreateBrokerFactoryAnonymous(IServiceProvider serviceProvider)
        {
            BrokerFactoryDelegate factory = delegate (string site)
            {
                if (string.Compare(site, "huobi", true) == 0)
                    return serviceProvider.GetService<Huobi>();
                else if (string.Compare(site, "binance", true) == 0)
                    return serviceProvider.GetService<Binance>();
                else
                    throw new InvalidOperationException($"不支持的站点：{site}");
            };
            return factory;
        }

        static void Main(string[] args)
        {
            /*
            var services = new ServiceCollection()
                      .AddSingleton<IBroker, Huobi>()
                      .BuildServiceProvider();
            var broker = services.GetService<IBroker>();
            broker.BuyLimit("USDT", "BTC", 10000, 1);
            */
            
            /*
            var services = new ServiceCollection()
                .AddLogging(config => config.AddConsole())
                      .AddSingleton<IBroker, Binance>()
                      .BuildServiceProvider();
            using (services)
            {
                var broker = services.GetService<IBroker>();
                broker.BuyLimit("USDT", "BTC", 10000, 1);
            }
            */

            var services = new ServiceCollection()
                .AddLogging(config => config.AddConsole())
                .AddSingleton<Huobi>()
                .AddSingleton<Binance>()
                // .AddTransient<BrokerFactoryDelegate>(serviceProvider => site => {
                //     if (string.Compare(site, "huobi", true) == 0)
                //         return serviceProvider.GetService<Huobi>();
                //     else if (string.Compare(site, "binance", true) == 0)
                //         return serviceProvider.GetService<Binance>();
                //     else
                //         throw new InvalidOperationException($"不支持的站点：{site}");
                // })
                // .AddTransient<BrokerFactoryDelegate>(CreateBrokerFactoryAnonymous)
                .AddTransient<BrokerFactoryDelegate>(CreateBrokerFactoryCaptured)
                .BuildServiceProvider();

            using (services)
            {
                var factory = services.GetService<BrokerFactoryDelegate>();
                var broker = factory("huobi");
                broker.BuyLimit("USDT", "BTC", 10000, 1);
                broker = factory("binance");
                broker.BuyLimit("USDT", "BTC", 10000, 1);
                // Console.ReadLine();
            }
        }
    }

    class BrokerFactoryDelegateWithCapturedVariable
    {
        public IServiceProvider serviceProvider;

        public IBroker BrokerFactoryDelegateImpl(string site)
        {
            if (string.Compare(site, "huobi", true) == 0)
                return serviceProvider.GetService<Huobi>();
            else if (string.Compare(site, "binance", true) == 0)
                return serviceProvider.GetService<Binance>();
            else
                throw new InvalidOperationException($"不支持的站点：{site}");
        }
    }

    public interface IBroker
    {
        void BuyLimit(string quote, string coin, decimal price, decimal volume);
    }

    public class Huobi : IBroker
    {
        public void BuyLimit(string quote, string coin, decimal price, decimal volume)
        {
            Console.WriteLine($"Huobi买单【{quote}-{coin}】, 买价：{price}，数量：{volume}");
        }
    }

    public class Binance : IBroker
    {
        private ILogger<Binance> _logger;
        public Binance(ILogger<Binance> logger)
        {
            _logger = logger;
        }

        public void BuyLimit(string quote, string coin, decimal price, decimal volume)
        {
            _logger.LogInformation($"Binance买单【{quote}-{coin}】, 买价：{price}，数量：{volume}");
        }
    }
}
