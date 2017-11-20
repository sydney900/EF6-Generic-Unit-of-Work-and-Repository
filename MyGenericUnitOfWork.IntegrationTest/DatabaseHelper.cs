using BussinessCore.Model;
using MyGenericUnitOfWork.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericUnitOfWork.IntegrationTest
{
    public class DatabaseHelper
    {
        public static List<Client> ListClient = new List<Client> {
            new Client { ClientName = "Joe", Email = "Joe@hotmail.com", ClientPassWord = "AA" },
            new Client { ClientName = "Marry", Email = "Marry@hotmail.com", ClientPassWord = "CC" },
            new Client { ClientName = "John", Email = "John@hotmail.com", ClientPassWord = "BB" }
        };

        public static List<Product> ListProduct = new List<Product>()
        {
            new Product { Name = "Bread" },
            new Product { Name = "Milk" }
        };

        public static void MigrateDbToLatest()
        {
            var configuration = new MyGenericUnitOfWork.Migrations.Configuration();
            var migrator = new System.Data.Entity.Migrations.DbMigrator(configuration);
            migrator.Update();
        }

        public static void Seed()
        {
            MyAppContext ctx = new MyAppContext();

            if (ctx.Clients.Any())
                return;

            ctx.Clients.AddRange(ListClient);
            ctx.Products.AddRange(ListProduct);

            ctx.SaveChanges();
        }
    }
}
