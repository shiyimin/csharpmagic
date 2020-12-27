// 源码位置：第六章\Linq2Sql\Program.cs
// 编译命令：dotnet run
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace Linq2Sql
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AdventureWorksContext())
            {
/*
                IQueryable<Customer> query = from c in db.Customer
                                             where c.TerritoryID == 1
                                             select c;
                foreach (var customer in query.Take(20))
                    Console.WriteLine($"{customer.TerritoryID}: {customer.AccountNumber}");
*/
                // query = db.Customer.Where(c => c.AccountNumber == "AW00000004");
                var query = db.Customer.Where(c => string.Compare(c.AccountNumber, "AW00000004") == 0);
                // var query = db.Customer.Where(c => Compare(c.AccountNumber, "AW00000004"));
                Console.WriteLine(query.First().CustomerID);
            }
        }

        static bool Compare(string left, string right) => string.Compare(left, right) == 0;
    }

    public class AdventureWorksContext : DbContext
    {
        static readonly ILoggerFactory s_loggerFactory = new LoggerFactory().AddConsole();
        public DbSet<Customer> Customer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Database=AdventureWorks;Server=.\\sqlexpress;Integrated Security=SSPI")
                      .UseLoggerFactory(s_loggerFactory);
    }

    [Table(nameof(Customer), Schema = "Sales")]
    public class Customer
    {
        public int CustomerID { get; set; }

        public int? PersonID { get; set; }

        public int? StoreID { get; set; }

        public int? TerritoryID { get; set; }

        public string AccountNumber { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set;}
    }
}
