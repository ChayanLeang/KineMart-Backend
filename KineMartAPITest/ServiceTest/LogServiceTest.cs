using KineMartAPI;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;

namespace KineMartAPITest.ServiceTest
{
    public class LogServiceTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private ILogService logService;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            logService = new LogService(repositoryWrapper);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfLog(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test]
        public async Task TestGetLogs()
        {
            var logs = await logService.GetLogsAsync();
            Assert.IsNotNull(logs);
            Assert.IsNotEmpty(logs);
            Assert.That(logs.Count(), Is.EqualTo(1));
            
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
