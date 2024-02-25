
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
    public class ImportControllerTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private IImportService importService;
        private IProductImportService productImportService;
        private ImportController importController;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productImportService = new ProductImportService(repositoryWrapper);
            importService = new ImportService(repositoryWrapper, productImportService);
            importController = new ImportController(importService, new NullLogger<ImportController>());
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProductImport(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public async Task TestGetImportByDateWithNoException()
        {
            var actionResult = await importController.GetImportByDateAsync(new DateFilter("2024-02-10",
                                                                                        "2024-02-11"));
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result=(actionResult as OkObjectResult)!.Value as IEnumerable<ImportViewModel>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.NotZero(result.Count());
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test, Order(2)]
        public async Task TestGetImportByDateWithException()
        {
            var actionResult = await importController.GetImportByDateAsync(null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(3)]
        public async Task TestGetTotalExpense()
        {
            var actionResult = await importController.GetTotalExpenseAsync();
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            double result = (double)(actionResult as OkObjectResult)!.Value!;
            Assert.IsNotNull(result);
            Assert.NotZero(result);
            Assert.That(result, Is.EqualTo(5));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
