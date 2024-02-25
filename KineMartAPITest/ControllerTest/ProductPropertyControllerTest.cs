
using KineMartAPI;
using KineMartAPI.Controllers;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace KineMartAPITest.ControllerTest
{
    public class ProductPropertyControllerTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private IProductPropertyService productPropertyService;
        private IImportService importService;
        private IProductImportService productImportService;
        private ProductPropertyController productPropertyController;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productImportService = new ProductImportService(repositoryWrapper);
            importService = new ImportService(repositoryWrapper, productImportService);
            productPropertyService = new ProductPropertyService(repositoryWrapper,importService,
                                                                          productImportService);
            productPropertyController = new ProductPropertyController(productPropertyService,
                                                new NullLogger<ProductPropertyController>());
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProductProperty(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public async Task TestAddProductPropertyAsyncWithNoException()
        {
            var importDto = new ImportDto()
            {
                SupplierId = 1,
                UserId="afafafafafaa",
                Products = new List<ProductImportDto>
                {
                    new ProductImportDto()
                    {
                        ProductId=2,
                        Cost=3,
                        Price=4,
                        Qty=10
                    }
                }
            };
            var actionResult = await productPropertyController.AddProductPropertyAsync(importDto);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(2)]
        public async Task TestAddProductPropertyAsyncWithException()
        {
            var actionResult = await productPropertyController.AddProductPropertyAsync(null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(3)]
        public async Task TestGetProductPropertiesAsync_WithNoOrderBy_WithNoSearch_WithPageNumber_WithNoPageSize()
        {
            var actionResult = await productPropertyController.GetProductPropertiesAsync(null!, null!, 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<ProductProperty>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test, Order(4)]
        public async Task TestGetProductProperties_WithOrderByName_WithNoSearch_WithPageNumber_WithNoPageSize()
        {
            var actionResult = await productPropertyController.GetProductPropertiesAsync("name", null!, 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<ProductProperty>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test, Order(5)]
        public async Task TestGetProductProperties_WithOrderById_WithSearch_WithPageNumber_WithNoPageSize()
        {
            var actionResult = await productPropertyController.GetProductPropertiesAsync("id","C", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<ProductProperty>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [OneTimeTearDown]
        public void ClearUp()
        {
            martDbContext.Database.EnsureDeleted();
        }
    }
}
