
using KineMartAPI;
using KineMartAPI.Controllers;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KineMartAPITest.ControllerTest
{
    public class LogControllerTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private ILogService logService;
        private LogController logController;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            logService = new LogService(repositoryWrapper);
            logController = new LogController(logService);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfLog(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test]
        public async Task TestGetLogs()
        {
            var actionResult = await logController.GetLogsAsync();
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result=(actionResult as OkObjectResult)!.Value as IEnumerable<Log>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
