using KineMartAPI;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;

namespace KineMartAPITest.ServiceTest
{
    public class ProductExportServiceTest
    {
        private MartDbContext martDbContext;
        private IProductExportService productExportService;
        private IRepositoryWrapper repositoryWrapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productExportService = new ProductExportService(repositoryWrapper);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProductExport(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test, Order(1)]
        public void TestAddProductExport1()
        {
            var productExport = new ProductExport()
            {
                ProductId = 2,
                ExportId = 1,
                Price = 2,
                Qty = 20,
                Amount = 40
            };
            Assert.That(async () => await productExportService.AddAndUpdateProductExportAsync(productExport, false), 
                                                                                               Throws.Nothing);
        }

        [Test, Order(2)]
        public async Task TestGetProductExportByCondition()
        {
            var productExport = await productExportService.GetProductExportByConditionAsync(1, 1);
            Assert.IsNotNull(productExport);
            Assert.That(productExport.ProductExId, Is.EqualTo(1));
            Assert.That(productExport.ProductId, Is.EqualTo(1));
            Assert.That(productExport.ExportId, Is.EqualTo(1));
            Assert.That(productExport.Qty, Is.EqualTo(5));
            Assert.That(productExport.Price, Is.EqualTo(2));
            Assert.That(productExport.Amount, Is.EqualTo(10));
        }

        [Test, Order(3)]
        public async Task TestAddProductExport2()
        {
            var currentProduct = await productExportService.GetProductExportByConditionAsync(1, 1);
            currentProduct.Price = 3;
            currentProduct.Amount = currentProduct.Qty * 3;
            Assert.That(async () => await productExportService.AddAndUpdateProductExportAsync(currentProduct, true), 
                                                                                               Throws.Nothing);
        }

        [Test, Order(4)]
        public async Task TestGetProductExports()
        {
            var productExports = await productExportService.GetProductExportsAsync();
            Assert.IsNotEmpty(productExports);
            Assert.That(productExports.Count(), Is.EqualTo(2));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
