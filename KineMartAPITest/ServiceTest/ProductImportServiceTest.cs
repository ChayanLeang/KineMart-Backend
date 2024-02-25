using KineMartAPI;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineMartAPITest.ServiceTest
{
    public class ProductImportServiceTest
    {
        private MartDbContext martDbContext;
        private IProductImportService productImportService;
        private IRepositoryWrapper repositoryWrapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productImportService = new ProductImportService(repositoryWrapper);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProductImport(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test, Order(1)]
        public void TestAddProductImport()
        {
            var productImport = new ProductImport()
            {
                ProductId = 1,
                SupplierId = 1,
                ImportId = 1,
                Cost = 1,
                Price = 2,
                Qty = 10,
                Amount = 10,
                Remain = 5,
                Date = "2024-02-10 12:03:34 PM"
            };
            Assert.That(async ()=> await productImportService.AddProductImportAsync(productImport),Throws.Nothing);
        }

        [Test, Order(2)]
        public async Task TestGetProductImports()
        {
            var products = await productImportService.GetProductImportsAsync();
            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products);
            Assert.That(products.Count(), Is.EqualTo(2));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
