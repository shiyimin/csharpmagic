// 源码位置：
// 编译运行：dotnet run --HUOBI-APIKEY=ASecretValue
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreDI
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Huobi>()
                .AddSingleton<IConfiguration>(config)
                .BuildServiceProvider();

            var huobi = serviceProvider.GetService<Huobi>();
            huobi.Initialize();
        }
    }

    public class Huobi
    {
        private readonly IConfiguration _config;
        public Huobi(IConfiguration config)
        {
            _config = config;
        }

        public void Initialize()
        {
            Console.WriteLine("配置信息：{0}", _config["HUOBI-APIKEY"]);
        }
    }
}
