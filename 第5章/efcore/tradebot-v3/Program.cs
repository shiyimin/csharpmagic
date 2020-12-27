using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace tradebot_v3
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AdventureWorksContext())
            {
                /*
                // 使用下面注释掉的一行会导致打印输出时报告NullReference异常
                // var person = context.Person.First();
                var person = context.Person.Include("Password").First();
                Console.WriteLine($"{person.FirstName},{person.LastName}: {person.Password.PasswordHash}");

                var order = context.SalesOrderHeader.Include("SalesOrderDetails").First();
                Console.WriteLine($"{order.SalesOrderID}: {order.SalesOrderDetails.Count}");

                var product = context.Product.Include("ProductProductPhotos").First();
                Console.WriteLine($"p - {product.ProductID}: {product.ProductProductPhotos.Count}");

                var photo = context.ProductPhoto.Include("ProductProductPhotos").First();
                Console.WriteLine($"pp - {photo.ProductPhotoID}: {photo.ProductProductPhotos.Count}, {photo.ThumbNailPhoto?.Length}");
                */

                var products = context.Product.Include("ProductProductPhotos")
                                      .Where(p => p.ProductNumber.StartsWith("CA"));
                foreach (var item in products)
                    Console.WriteLine($"item - {item.ProductID}: {item.ProductProductPhotos.Count}");

                var orders = context.SalesOrderHeader
                                    .Include("SalesOrderDetails")
                                    .Include("SalesOrderDetails.Product")
                                    .Where(o => o.SalesOrderID > 43659 && o.SalesOrderID < 43662);
                
                foreach (var item in orders)
                    Console.WriteLine($"order - {item.SalesOrderID}: {item.SalesOrderDetails[0].Product.Name}。");

                var employees = context.Set<ManagerEmployee>().FromSqlRaw("EXEC uspGetManagerEmployees 2");
                foreach (var item in employees)
                    Console.WriteLine($"{item.ManagerFirstName} {item.ManagerLastName}");
            }
        }
    }
}
