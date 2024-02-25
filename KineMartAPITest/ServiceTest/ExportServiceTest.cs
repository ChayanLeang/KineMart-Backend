using KineMartAPI;
using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;

namespace KineMartAPITest.ServiceTest
{
    public class ExportServiceTest
    {
        private MartDbContext martDbContext;
        private IProductExportService productExportService;
        private IExportService exportService;
        private IRepositoryWrapper repositoryWrapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productExportService = new ProductExportService(repositoryWrapper);
            exportService = new ExportService(repositoryWrapper, productExportService);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProductExport(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public void TestAddExport()
        {
            var export = new Export()
            {
                UserId= "aafafafafafaaf",
                Date = "2024-02-11"
            };
            Assert.That(async () => await exportService.AddExportAsync(export), Throws.Nothing);
        }

        [Test, Order(2)]
        public async Task TestGetExportByDateWithNoException()
        {
            var exports = await exportService.GetExportByDateAsync(new DateFilter("2024-02-10","2024-02-11"));
            Assert.IsNotNull(exports);
            Assert.IsNotEmpty(exports);
            Assert.That(exports.Count(), Is.EqualTo(2));
            var export = exports.FirstOrDefault()!;
            Assert.That(export.ExportId, Is.EqualTo(1));
            Assert.That(export.Date, Is.EqualTo("2024-02-10"));
            var record = export.ExportRecords.FirstOrDefault()!;
            Assert.IsNotNull(record);
            Assert.That(record.ProductName, Is.EqualTo("Coca"));
            Assert.That(record.Price, Is.EqualTo(2));
            Assert.That(record.Qty, Is.EqualTo(5));
        }

        [Test, Order(3)]
        public void TestGetExportByDateWithException1()
        {
            Assert.That(async () => await exportService.GetExportByDateAsync(new DateFilter("12-02-2024", 
                                          "2024-02-11")), Throws.Exception.TypeOf<ExceptionBase>().With.Message
                                          .EqualTo("StartDate should be yyyy-MM-dd"));
        }

        [Test, Order(4)]
        public void TestGetExportByDateWithException2()
        {
            Assert.That(async () => await exportService.GetExportByDateAsync(new DateFilter("2024-02-11", 
                                          "13-02-2024")), Throws.Exception.TypeOf<ExceptionBase>().With.Message
                                          .EqualTo("EndDate should be yyyy-MM-dd"));
        }

        [Test, Order(5)]
        public void TestGetExportByDateWithException3()
        {
            Assert.That(async () => await exportService.GetExportByDateAsync(new DateFilter("2024-02-11", 
                                          "2024-02-10")), Throws.Exception.TypeOf<ExceptionBase>().With.Message
                                          .EqualTo("StartDate must be smaller than EndDate"));
        }

        [Test, Order(6)]
        public async Task TestGetLastExport()
        {
            var export = await exportService.GetLastExportAsync();
            Assert.IsNotNull(export);
            Assert.That(export.ExportId, Is.EqualTo(2));
            Assert.That(export.Date, Is.EqualTo("2024-02-11"));
        }

        [Test, Order(7)]
        public async Task TestGetTotalRevenus()
        {
            var total = await exportService.GetTotalRevenusAsync();
            Assert.That(total, Is.EqualTo(10));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
