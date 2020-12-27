using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace TradeBot.Strategy.Band.Db
{
    public class DesignTimeBandTradeContextFactory : IDesignTimeDbContextFactory<BandTradeContext>
    {
        public BandTradeContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            ILoggerFactory loggerFactory
                = LoggerFactory.Create(builder => { builder.AddConsole(); });
            return new BandTradeContext(configuration, loggerFactory);
        }
    }
}
