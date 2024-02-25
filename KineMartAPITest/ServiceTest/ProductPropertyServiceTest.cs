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
    public class ProductPropertyServiceTest
    {
        private MartDbContext martDbContext;
        private IProductPropertyService productPropertyService;
        private IRepositoryWrapper repositoryWrapper;
        private IImportService importService;
        private IProductImportService productImportService;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            productImportService = new ProductImportService(repositoryWrapper);
            importService = new ImportService(repositoryWrapper, productImportService);
            productPropertyService = new ProductPropertyService(repositoryWrapper, importService,
                                                                           productImportService);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfProductProperty(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public void TestAddProductProperty1()
        {
            Assert.That(async () => await productPropertyService.AddProductPropertyAsync(ImportDto(1,1,2)),
                                                                                      Throws.Nothing);
        }
        [Test, Order(2)]
        public void TestAddProductProperty2()
        {
            Assert.That(async () => await productPropertyService.AddProductPropertyAsync(ImportDto(2,1,2)), 
                                                                                      Throws.Nothing);
        }

        [Test, Order(3)]
        public void TestAddProductProperty3()
        {
            var import = new Import()
            {
                UserId= "aafafafafafaaf",
                Date = "2024-02-08"
            };
            martDbContext.Imports.Add(import);
            martDbContext.SaveChanges();
            Assert.That(async () => await productPropertyService.AddProductPropertyAsync(ImportDto(1,2,3)), 
                                                                                      Throws.Nothing);
        }

        [Test, Order(4)]
        public async Task TestGetProductProperties_WithNoOrderBy_WithNoSearch_WithNoPageNumber_WithNoPageSize()
        {
            var productProperties = await productPropertyService.GetProductPropertiesAsync("","",0,0);
            Assert.IsNotNull(productProperties);
            Assert.IsEmpty(productProperties);
            Assert.That(productProperties.Count(), Is.EqualTo(0));
        }

        [Test, Order(5)]
        public async Task TestGetProductProperties_WithNoOrderBy_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var productProperties = await productPropertyService.GetProductPropertiesAsync("", "", 1, 5);
            Assert.IsNotNull(productProperties);
            Assert.IsNotEmpty(productProperties);
            Assert.That(productProperties.Count(), Is.EqualTo(2));
        }

        [Test, Order(6)]
        public async Task TestGetProductProperties_WithOrderByName_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var productProperties = await productPropertyService.GetProductPropertiesAsync("name", "", 1, 5);
            Assert.IsNotNull(productProperties);
            Assert.IsNotEmpty(productProperties);
            Assert.That(productProperties.Count(), Is.EqualTo(2));
            var productProperty = productProperties.FirstOrDefault()!;
            Assert.That(productProperty.ProductProId, Is.EqualTo(1));
            Assert.That(productProperty.ProductId, Is.EqualTo(1));
            Assert.That(productProperty.Product.ProductName, Is.EqualTo("Coca"));
            Assert.That(productProperty.Cost, Is.EqualTo(2));
            Assert.That(productProperty.Price, Is.EqualTo(3));
            Assert.That(productProperty.Qty, Is.EqualTo(30));
        }

        [Test, Order(7)]
        public async Task TestGetProductProperties_WithOrderById_WithSearch_WithPageNumber_WithPageSize()
        {
            var productProperties = await productPropertyService.GetProductPropertiesAsync("id", "P", 1, 5);
            Assert.IsNotNull(productProperties);
            Assert.IsNotEmpty(productProperties);
            Assert.That(productProperties.Count(), Is.EqualTo(1));
            var productProperty = productProperties.FirstOrDefault()!;
            Assert.That(productProperty.ProductProId, Is.EqualTo(2));
            Assert.That(productProperty.ProductId, Is.EqualTo(2));
            Assert.That(productProperty.Product.ProductName, Is.EqualTo("Pepsi"));
            Assert.That(productProperty.Cost, Is.EqualTo(1));
            Assert.That(productProperty.Price, Is.EqualTo(2));
            Assert.That(productProperty.Qty, Is.EqualTo(10));
        }

        [Test, Order(8)]
        public async Task TestGetProductProperyByIdWithNoException()
        {
            var productProperty = await productPropertyService.GetProductPropertyByIdAsync(1);
            Assert.IsNotNull(productProperty);
            Assert.That(productProperty.ProductProId, Is.EqualTo(1));
            Assert.That(productProperty.Product.ProductName, Is.EqualTo("Coca"));
            Assert.That(productProperty.Cost, Is.EqualTo(2));
            Assert.That(productProperty.Price, Is.EqualTo(3));
            Assert.That(productProperty.Qty, Is.EqualTo(30));
        }

        [Test, Order(9)]
        public void TestGetProductProperyByIdWithException()
        {
            Assert.That(async () => await productPropertyService.GetProductPropertyByIdAsync(3), Throws.Exception
                                          .TypeOf<NotFoundException>().With.Message.EqualTo
                                          ("ProductProperty with id = 3 didn't find"));
        }

        [Test, Order(10)]
        public void TestUpdateProductProperty1()
        {
            var productProperty = new ProductProperty()
            {
                ProductId = 1,
                Cost = 1,
                Price = 2,
                Qty = 10
            };
            Assert.That(async () => await productPropertyService.UpdateProductPropertyAsync(1,0,false, 
                                                                     productProperty),Throws.Nothing);
        }

        [Test, Order(11)]
        public void TestUpdateProductProperty2()
        {
            Assert.That(async () => await productPropertyService.UpdateProductPropertyAsync(1, 5, true, 
                                                          new ProductProperty()), Throws.Nothing);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }

        private ImportDto ImportDto(int pro_id,double cost,double price)
        {
            return new ImportDto()
            {
                SupplierId = 1,
                UserId= "aafafafafafaaf",
                Products = new List<ProductImportDto>()
                {
                    new ProductImportDto()
                    {
                        ProductId=pro_id,
                        Cost=cost,
                        Price=price,
                        Qty=10
                    }
                }
            };
        }
    }
}
