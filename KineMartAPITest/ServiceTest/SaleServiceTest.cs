using KineMartAPI;
using KineMartAPI.Exceptions;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;

namespace KineMartAPITest.ServiceTest
{
    public class SaleServiceTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private ISaleService saleService;
        private IProductImportService productImportService;
        private IProductExportService productExportService;
        private IExportService exportService;
        private IImportService importService;
        private IProductPropertyService productPropertyService;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productExportService = new ProductExportService(repositoryWrapper);
            productImportService = new ProductImportService(repositoryWrapper);
            importService = new ImportService(repositoryWrapper, productImportService);
            exportService = new ExportService(repositoryWrapper, productExportService);
            productPropertyService = new ProductPropertyService(repositoryWrapper, importService, 
                                                                           productImportService);
            saleService = new SaleService(exportService,productExportService,productPropertyService);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfSale(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public void TestSellWithNoException1()
        {
            Assert.That(async () => await saleService.SellAsync(SaleDto(5)),Throws.Nothing);
        }

        [Test, Order(2)]
        public void TestSellWithNoException2()
        {
            var export = new Export()
            {
                UserId= "aafafafafafaaf",
                Date = "2024-02-11"
            };
            martDbContext.Exports.Add(export);
            martDbContext.SaveChanges();
            Assert.That(async () => await saleService.SellAsync(SaleDto(5)), Throws.Nothing);
        }

        [Test, Order(3)]
        public void TestSellWithException()
        {
            Assert.That(async () => await saleService.SellAsync(SaleDto(20)), Throws.Exception.TypeOf<ExceptionBase>()
                                          .With.Message.EqualTo("Coca wasn't enough in stock"));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }

        private SaleDto SaleDto(int qty)
        {
            return new SaleDto()
            {
                UserId= "aafafafafafaaf",
                Products = new List<ProductSaleDto>
                {
                    new ProductSaleDto()
                    {
                        ProductProId=1,
                        Qty=qty
                    }
                }
            };
        }
    }
}
