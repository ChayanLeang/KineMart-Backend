using KineMartAPI.ModelEntities;
using KineMartAPI;
using NUnit.Framework.Constraints;
using Microsoft.AspNetCore.Identity;
using KineMartAPI.ModelDtos;

namespace KineMartAPITest
{
    public class SeedDatabase
    {
        public static void SeedDatabaseOfCategory(MartDbContext martDbContext)
        {
            var category = new Category()
            {
                CategoryName = "Food",
                IsActive = true
            };
            martDbContext.Categories.Add(category);
        }

        public static void SeedDatabaseOfProduct(MartDbContext martDbContext)
        {
            SeedDatabaseOfCategory(martDbContext);
            var products = new List<Product>()
            {
                new Product()
                {
                    CategoryId = 1,
                    ProductName = "Coca",
                    IsActive = true
                },
                new Product()
                {
                    CategoryId = 1,
                    ProductName = "Pepsi",
                    IsActive = true
                }
            };
            martDbContext.Products.AddRange(products);
        }

        public static void SeedDatabaseOfSupplier(MartDbContext martDbContext)
        {
            var supplier = new Supplier()
            {
                CompanyName = "Food Company",
                Owner = "Light Yagami",
                Address = "Tokyo",
                PhoneNumber = "098222111",
                IsActive = true
            };
            martDbContext.Suppliers.Add(supplier);
        }

        public static void SeedDatabaseOfImport(MartDbContext martDbContext)
        {
            SeedDatabaseOfUser(martDbContext);
            var import = new Import()
            {
                UserId = "aafafafafafaaf",
                Date = "2024-02-10"
            };
            martDbContext.Imports.Add(import);
        }

        public static void SeedDatabaseOfExport(MartDbContext martDbContext,bool isSale)
        {
            if (!isSale)
            {
                SeedDatabaseOfUser(martDbContext);
            }
            var export = new Export()
            {
                UserId= "aafafafafafaaf",
                Date = "2024-02-10"
            };
            martDbContext.Exports.Add(export);
        }

        public static void SeedDatabaseOfProductImport(MartDbContext martDbContext)
        {
            SeedDatabaseOfProduct(martDbContext);
            SeedDatabaseOfSupplier(martDbContext);
            SeedDatabaseOfImport(martDbContext);
            var productImport = new ProductImport()
            {
                ProductId=1,
                ImportId=1,
                SupplierId=1,
                Cost=1,
                Price=2,
                Qty=5,
                Remain=0,
                Amount=5,
                Date="2024-02-10 12:00:00 PM"
            };
            martDbContext.ProductImports.Add(productImport);
        }

        public static void SeedDatabaseOfProductExport(MartDbContext martDbContext)
        {
            SeedDatabaseOfProduct(martDbContext);
            SeedDatabaseOfExport(martDbContext,false);
            var productExport = new ProductExport()
            {
                ProductId = 1,
                ExportId=1,
                Price = 2,
                Qty = 5,
                Amount = 10,
            };
            martDbContext.ProductExports.Add(productExport);
        }

        public static void SeedDatabaseOfLog(MartDbContext martDbContext)
        {
            var log = new Log()
            {
                Message = "This is GetLog()",
                MessageTemplate = "This is GetLog()",
                Level = "Information",
                TimeStamp = DateTime.Now,
                Exception = "Invalid Log",
                Properties = "N/A"
            };
            martDbContext.Logs.Add(log);
        }

        public static void SeedDatabaseOfProductProperty(MartDbContext martDbContext)
        {
            SeedDatabaseOfImport(martDbContext);
            SeedDatabaseOfProduct(martDbContext);
            var productProperty = new ProductProperty()
            {
                ProductId = 1,
                Cost = 1,
                Price = 2,
                Qty = 10
            };
            martDbContext.ProductProperties.Add(productProperty);
        }

        public static void SeedDatabaseOfSale(MartDbContext martDbContext)
        {
            SeedDatabaseOfExport(martDbContext,true);
            SeedDatabaseOfProductProperty(martDbContext);
        }

        public static void SeedDatabaseOfUser(MartDbContext martDbContext)
        {
            var user = new ApplicationUser()
            {
                Id= "aafafafafafaaf",
                UserName = "Yuki",
                Email = "Yuki@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = "097233111",
            };
            martDbContext.Users.Add(user);
        }
    }
}
