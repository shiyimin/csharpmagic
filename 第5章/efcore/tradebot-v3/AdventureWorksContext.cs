using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class AdventureWorksContext : DbContext
{
    public DbSet<Person> Person { get; set; } = null!;

    public DbSet<Password> Password { get; set; } = null!;

    public DbSet<SalesOrderHeader> SalesOrderHeader { get; set; } = null!;

    public DbSet<SalesOrderDetail> SalesOrderDetail { get; set; } = null!;

    public DbSet<ProductPhoto> ProductPhoto { get; set; } = null!;

    public DbSet<Product> Product { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasKey(p => p.BusinessEntityID);
        modelBuilder.Entity<Password>().HasKey(p => p.BusinessEntityID);
        modelBuilder.Entity<Person>().HasOne("Password").WithOne().HasForeignKey("Password", "BusinessEntityID");
        modelBuilder.Entity<SalesOrderHeader>().HasKey(p => p.SalesOrderID);
        modelBuilder.Entity<SalesOrderDetail>().HasKey(p => p.SalesOrderDetailID);
        modelBuilder.Entity<SalesOrderHeader>().HasMany(o => o.SalesOrderDetails).WithOne().HasForeignKey(o => o.SalesOrderID);
        modelBuilder.Entity<SalesOrderDetail>().HasOne(o => o.Product).WithMany().HasForeignKey(o => o.ProductID);
        
        modelBuilder.Entity<Product>().HasKey(p => p.ProductID);
        modelBuilder.Entity<ProductPhoto>().HasKey(p => p.ProductPhotoID);
        modelBuilder.Entity<ProductProductPhoto>().HasKey(ppp => new {
            ProductID = ppp.ProductID, ProductPhotoID = ppp.ProductPhotoID });

        modelBuilder.Entity<ProductProductPhoto>().HasOne(ppp => ppp.Product)
            .WithMany(p => p.ProductProductPhotos).HasForeignKey(ppp => ppp.ProductID);
        modelBuilder.Entity<ProductProductPhoto>().HasOne(ppp => ppp.ProductPhoto)
            .WithMany(pp => pp.ProductProductPhotos).HasForeignKey(ppp => ppp.ProductPhotoID);
        //modelBuilder.Entity<Product>().HasMany(p => p.ProductPhotos)
        //    .WithMany(p => p.Products)            
        //    .Map(mapping => mapping
        //    .ToTable("ProductProductPhoto", "Production")
        //    .MapLeftKey("ProductID")
        //    .MapRightKey("ProductPhotoID"));
    }
    
    static readonly ILoggerFactory s_loggerFactory 
        = LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Database=AdventureWorks;Server=.\\sqlexpress;Integrated Security=SSPI")
                  .UseLoggerFactory(s_loggerFactory);
}