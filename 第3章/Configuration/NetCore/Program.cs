// 源码位置：第四章/Configuration/NetCore/Program.cs
// 编译：dotnet build
// 运行：
//     1. Mac: 
//           export CSHARP_FROMENV=EnvValue dotnet run --CmdKey1=CmdValue1
//           或者
//           export CSHARP_FROMENV=EnvValue dotnet run -d=CmdValue1
//     2. Windows: 
//           set CSHARP_FROMENV=EnvValue
//           dotnet run --CmdKey1=CmdValue1
//           或者
//           dotnet run -d=CmdValue1
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var dict = new Dictionary<string, string>
            {
                {"mckey1", "mc-value-1"},
                {"mckey2", "mc-value-2"}
            };
            var switchMapping = new Dictionary<string, string>
            {
                {"-d", "CmdKey1"},
                {"--k", "CmdKey1"}
            };
            
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(dict)
                .AddEnvironmentVariables("CSHARP_")
                // .AddCommandLine(args)
                .AddCommandLine(args, switchMapping)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            if (!string.IsNullOrEmpty(environment))
                builder = builder.AddJsonFile($"appsettings.{environment}.json");
            var config = builder.Build();

            Console.WriteLine("内存配置：{0}", config["mckey1"]);
            Console.WriteLine("命令行配置：{0}", config["CmdKey1"]);
            Console.WriteLine("环境变量配置：{0}", config["FromEnv"]);
            Console.WriteLine("JSON配置：{0}", config["JsonKey1"]);
            Console.WriteLine("JSON层级配置：{0}", config["Logging:Debug:LogLevel:System"]);
            
            var option = new Option();
            config.GetSection("option").Bind(option);
            Console.WriteLine($"站点：{option.Site}，方向：{option.IsBuy}，价格：{option.Price}");
        }
    }

    // 从命令行中传入：dotnet run --option:site=huobi --option:isbuy=true --option:price=9800
    class Option
    {
        public string Site { get; set; }

        public string Quote { get; set; }

        public string Coin { get; set; }

        public bool IsBuy { get; set; }

        public decimal Price { get; set; }

        public decimal Volumn { get; set; }
    }
}
