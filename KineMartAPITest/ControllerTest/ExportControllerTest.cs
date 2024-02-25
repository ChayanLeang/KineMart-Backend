
using KineMartAPI;
using KineMartAPI.Controllers;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;
using KineMartAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace KineMartAPITest.ControllerTest
{
    public class ExportControllerTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private IExportService exportService;
        private IProductExportService productExportService;
        private ExportController exportController;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productExportService = new ProductExportService(repositoryWrapper);
            exportService = new ExportService(repositoryWrapper, productExportService);
            exportController = new ExportController(exportService, new NullLogger<ExportController>());
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProductExport(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public async Task TestGetExportByDateWithNoException()
        {
            var actionResult = await exportController.GetExportByDateAsync(new DateFilter("2024-02-09", 
                                                                                        "2024-02-10"));
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<ExportViewModel>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.NotZero(result.Count());
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test, Order(2)]
        public async Task TestGetExportByDateWithException()
        {
            var actionResult = await exportController.GetExportByDateAsync(null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(3)]
        public async Task TestGetTotalRevenus()
        {
            var actionResult = await exportController.GetTotalRevenusAsync();
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            double result = (double)(actionResult as OkObjectResult)!.Value!;
            Assert.That(result, Is.EqualTo(10));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
