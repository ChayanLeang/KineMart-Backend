using AutoMapper;
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
    public class ProductControllerTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private IProductService productService;
        private ProductController productController;
        private IMapper mapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productService = new ProductService(repositoryWrapper);
            mapper = MapperConfigInit.GetMapper();
            productController = new ProductController(productService,new NullLogger<ProductController>(), mapper);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProduct(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public async Task TestAddProductWithNoEexception()
        {
            var actionResult = await productController.AddProductAsync(ProductDtos("Sting"));
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(2)]
        public async Task TestAddProductWithEexception()
        {
            var actionResult = await productController.AddProductAsync(null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(3)]
        public async Task TestGetProducts_WithNoOrderBy_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await productController.GetProductsAsync(null!,null!, 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Product>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test, Order(4)]
        public async Task TestGetProducts_WithOrderByName_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await productController.GetProductsAsync("name", null!, 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Product>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test, Order(5)]
        public async Task TestGetProducts_WithOrderById_WithSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await productController.GetProductsAsync("id","S", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Product>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test, Order(6)]
        public async Task TestUpdateProductWithNoException()
        {
            var actionResult = await productController.UpdateProductAsync(1, ProductDtos("IZE").First());
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public async Task TestUpdateProductWithException()
        {
            var actionResult = await productController.UpdateProductAsync(2, null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(8)]
        public async Task TestGetNumberOfProducts()
        {
            var actionResult = await productController.GetNumberOfProductsAsync();
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            int result = (int)(actionResult as OkObjectResult)!.Value!;
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(3));
        }

        [OneTimeTearDown]
        public void ClearUp()
        {
            martDbContext.Database.EnsureDeleted();
        }

        private List<ProductDto> ProductDtos(string pro_name)
        {
            return new List<ProductDto>
            {
                new ProductDto()
                {
                    CategoryId = 1,
                    ProductName = pro_name,
                    IsActive = true
                }
            };
        }
    }
}
