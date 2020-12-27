using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class TradeHistoryContext : DbContext
{
    private IConfiguration _config;
    public TradeHistoryContext(IConfiguration config)
    {
        _config = config;
    }
    public DbSet<DbPlacedOrder> DbPlacedOrders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbPlacedOrder>().Property(x => x.AskPrice).HasColumnType("decimal(16, 8)");
        modelBuilder.Entity<DbPlacedOrder>().Property(x => x.BidPrice).HasColumnType("decimal(16, 8)");
        modelBuilder.Entity<DbPlacedOrder>().Property(x => x.TradingVolume).HasColumnType("decimal(16, 8)");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(_config["db"]);
}