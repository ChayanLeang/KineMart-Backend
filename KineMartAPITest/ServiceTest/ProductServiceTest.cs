using KineMartAPI;
using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;

namespace KineMartAPITest.ServiceTest
{
    public class ProductServiceTest
    {
        private MartDbContext martDbContext;
        private IProductService productService;
        private IRepositoryWrapper repositoryWrapper;

        [OneTimeSetUp]
        public void Setup()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productService = new ProductService(repositoryWrapper);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProduct(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test, Order(1)]
        public void TestAddProductWithNoException()
        {
            var products = Products("IZE");
            Assert.That(async () => await productService.AddProductAsync(products), Throws.Nothing);
        }

        [Test, Order(2)]
        public void TestAddProductWithException()
        {
            var products = Products("Coca");
            Assert.That(async () => await productService.AddProductAsync(products), Throws.Exception
                                          .TypeOf<UniqueException>().With.Message.EqualTo
                                          ("ProductName (Coca) was already exist"));
        }

        [Test,Order(3)]
        public async Task TestGetProducts_WithNoOrderBy_WithNoSearch_WithNoPageNumber_WithNoPagesize()
        {
            var products = await productService.GetProductsAsync(null!,null!,0,0);
            Assert.IsNotNull(products);
            Assert.IsEmpty(products);
            Assert.That(products.Count(), Is.EqualTo(0));
        }

        [Test, Order(4)]
        public async Task TestGetProducts_WithNoOrderBy_WithNoSearch_WithPageNumber_WithPagesize()
        {
            var products = await productService.GetProductsAsync(null!,null!, 1, 5);
            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products);
            Assert.That(products.Count(), Is.EqualTo(3));
        }

        [Test, Order(5)]
        public async Task TestGetProducts_WithOrderByName_WithNoSearch_WithPageNumber_WithPagesize()
        {
            var products = await productService.GetProductsAsync("name", null!, 1, 5);
            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products);
            Assert.That(products.Count(), Is.EqualTo(3));
            var product = products.FirstOrDefault()!;
            Assert.That(product.ProductId, Is.EqualTo(1));
            Assert.That(product.CategoryId, Is.EqualTo(1));
            Assert.That(product.ProductName, Is.EqualTo("Coca"));
            Assert.That(product.IsActive, Is.EqualTo(true));
        }

        [Test, Order(6)]
        public async Task TestGetProducts_WithOrderById_WithSearch_WithPageNumber_WithPagesize()
        {
            var products = await productService.GetProductsAsync("id", "P", 1, 5);
            Assert.IsNotNull(products);
            Assert.IsNotEmpty(products);
            Assert.That(products.Count(), Is.EqualTo(1));
            var product = products.FirstOrDefault()!;
            Assert.That(product.ProductId, Is.EqualTo(2));
            Assert.That(product.CategoryId, Is.EqualTo(1));
            Assert.That(product.ProductName, Is.EqualTo("Pepsi"));
            Assert.That(product.IsActive, Is.EqualTo(true));
        }

        [Test, Order(7)]
        public void TestUpdateProductWithNoException()
        {
            Assert.That(async () => await productService.UpdateProductAsync(1, Products("Sting").First()),
                                                                                     Throws.Nothing);
        }

        [Test, Order(8)]
        public void TestUpdateProductWithException1()
        {
            Assert.That(async () => await productService.UpdateProductAsync(4, Products("IZE").First()), 
                                          Throws.Exception.TypeOf<NotFoundException>().With.Message.EqualTo
                                          ("Product with id = 4 didn't find"));
        }

        [Test, Order(9)]
        public void TestUpdateProductWithException2()
        {
            Assert.That(async () => await productService.UpdateProductAsync(2, Products("IZE").First()),
                                          Throws.Exception.TypeOf<UniqueException>().With.Message.EqualTo
                                          ("ProductName (IZE) was already exist"));
        }

        [Test, Order(10)]
        public async Task TestGetNumberOfProducts()
        {
            var result = await productService.GetNumberOfProductsAsync();
            Assert.That(result, Is.EqualTo(3));
        }

        [OneTimeTearDown]
        public void ClearUp()
        {
            martDbContext.Database.EnsureDeleted();
        }

        private List<Product> Products(string name)
        {
            return new List<Product>
            {
                new Product()
                {
                    CategoryId=1,
                    ProductName=name,
                    IsActive=true
                }
            };
        }
    }
}
