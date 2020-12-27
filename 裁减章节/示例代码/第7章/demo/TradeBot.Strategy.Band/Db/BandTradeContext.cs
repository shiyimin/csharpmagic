using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TradeBot.Strategy.Band.Db
{
    public class BandTradeContext : DbContext
    {
        private IConfiguration _config;
        private ILoggerFactory _loggerFactory;

        public BandTradeContext(IConfiguration config, ILoggerFactory loggerFactory) : 
            this(new DbContextOptions<BandTradeContext>(), config, loggerFactory)
        {
        }

        // 支持InMemory数据库测试
        public BandTradeContext(DbContextOptions<BandTradeContext> options)
            : this(options, null, null) { }

        public BandTradeContext(DbContextOptions<BandTradeContext> options, IConfiguration config, ILoggerFactory loggerFactory)
            : base(options)
        {
            _config = config;
            _loggerFactory = loggerFactory;
        }

        public DbSet<DbPlacedOrder> DbPlacedOrders { get; set; } = null!;

        public DbSet<BandStrategy> BandStrategies { get; set; } = null!;

        public DbSet<UserTradeApiSetting> UserTradeApiSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbPlacedOrder>().Property(x => x.AskPrice).HasColumnType("decimal(16, 8)");
            modelBuilder.Entity<DbPlacedOrder>().Property(x => x.BidPrice).HasColumnType("decimal(16, 8)");
            modelBuilder.Entity<DbPlacedOrder>().Property(x => x.TradingVolume).HasColumnType("decimal(16, 8)");

            modelBuilder.Entity<BandStrategy>().Property(x => x.BidPrice).HasColumnType("decimal(16, 8)");
            modelBuilder.Entity<BandStrategy>().Property(x => x.TradeVolume).HasColumnType("decimal(16, 8)");
            modelBuilder.Entity<BandStrategy>().Property(x => x.AskProfit).HasColumnType("decimal(16, 8)");
            modelBuilder.Entity<BandStrategy>().Property(x => x.AskPrice).HasColumnType("decimal(16, 8)");
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_config["db"])
                      .UseLoggerFactory(_loggerFactory);
        */

        // 支持InMemory数据库测试
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(_config["db"])
                       .UseLoggerFactory(_loggerFactory);
            }
        }
    }
}
