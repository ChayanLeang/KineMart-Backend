using KineMartAPI;
using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;

namespace KineMartAPITest.ServiceTest
{
    public class ImportServiceTest
    {
        private MartDbContext martDbContext;
        private IImportService importService;
        private IRepositoryWrapper repositoryWrapper;
        private IProductImportService productImportService;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productImportService = new ProductImportService(repositoryWrapper);
            importService = new ImportService(repositoryWrapper, productImportService);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProductImport(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public void TestAddImport()
        {
            var import = new Import()
            {
                UserId = "aafafafafafaaf",
                Date = "2024-02-11"
            };
            Assert.That(async () => await importService.AddImportAsync(import), Throws.Nothing);
        }

        

        [Test, Order(2)]
        public async Task TestGetImportByDateWithNoException()
        {
            var imports = await importService.GetImportByDateAsync(new DateFilter("2024-02-10","2024-02-11"));
            Assert.IsNotNull(imports);
            Assert.That(imports.Count(), Is.EqualTo(2));
            var import = imports.FirstOrDefault()!;
            Assert.That(import.ImportId, Is.EqualTo(1));
            Assert.That(import.Date, Is.EqualTo("2024-02-10"));
            var record = import.ImportRecords.FirstOrDefault()!;
            Assert.That(record.CompanyName, Is.EqualTo("Food Company"));
            Assert.That(record.Date, Is.EqualTo("2024-02-10 12:00:00 PM"));
            var product = record.Products.FirstOrDefault()!;
            Assert.That(product.ProductName, Is.EqualTo("Coca"));
            Assert.That(product.Price, Is.EqualTo(2));
            Assert.That(product.Cost, Is.EqualTo(1));
            Assert.That(product.Qty, Is.EqualTo(5));
            Assert.That(product.Remain, Is.EqualTo(0));
        }

        [Test, Order(3)]
        public void TestGetImportByDateWithException1()
        {
            Assert.That(async () => await importService.GetImportByDateAsync(new DateFilter("10-02-2024", 
                                          "2024-02-11")), Throws.Exception.TypeOf<ExceptionBase>().With.Message
                                          .EqualTo("StartDate should be yyyy-MM-dd"));
        }

        [Test, Order(4)]
        public void TestGetImportByDateWithException2()
        {
            Assert.That(async () => await importService.GetImportByDateAsync(new DateFilter("2024-02-11", 
                                          "12-02-2024")), Throws.Exception.TypeOf<ExceptionBase>().With.Message
                                          .EqualTo("EndDate should be yyyy-MM-dd"));
        }

        [Test, Order(5)]
        public void TestGetImportByDateWithException3()
        {
            Assert.That(async () => await importService.GetImportByDateAsync(new DateFilter("2024-02-11", 
                                          "2024-02-10")), Throws.Exception.TypeOf<ExceptionBase>().With.Message
                                          .EqualTo("StartDate must be smaller than EndDate"));
        }

        [Test, Order(6)]
        public async Task TestGetLastImport()
        {
            var import = await importService.GetLastImportAsync();
            Assert.IsNotNull(import);
            Assert.That(import.ImportId, Is.EqualTo(2));
            Assert.That(import.Date, Is.EqualTo("2024-02-11"));
        }

        [Test, Order(7)]
        public async Task TestGetTotalExpense()
        {
            var total = await importService.GetTotalExpenseAsync();
            Assert.That(total, Is.EqualTo(5));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
