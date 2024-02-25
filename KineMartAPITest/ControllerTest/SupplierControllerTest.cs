
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
    public class SupplierControllerTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private ISupplierService supplierService;
        private IMapper mapper;
        private SupplierController supplierController;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            supplierService = new SupplierService(repositoryWrapper);
            mapper = MapperConfigInit.GetMapper();
            supplierController = new SupplierController(supplierService, new NullLogger<SupplierController>(), 
                                                                                                      mapper);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfSupplier(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public async Task TestAddSupplierWithNoException()
        {
            var actionResult = await supplierController.AddSupplierAsync(SupplierDtos("Swe Company","097344555"));
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(2)]
        public async Task TestAddSupplierWithException()
        {
            var actionResult = await supplierController.AddSupplierAsync(null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(3)]
        public async Task TestGetSuppliers_WithNoOrderBy_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await supplierController.GetSuppliersAsync(null!,null!, 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Supplier>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test, Order(4)]
        public async Task TestGetSuppliers_WithOrderByName_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await supplierController.GetSuppliersAsync("name", null!, 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Supplier>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test, Order(5)]
        public async Task TestGetSuppliers_WithOrderById_WithSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await supplierController.GetSuppliersAsync("id","S", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Supplier>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test, Order(6)]
        public async Task TestUpdateSupplierWithNoException()
        {
            var actionResult = await supplierController.UpdateSupplierAsync(1,SupplierDtos("Love Company",
                                                                                    "096344222").First());
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public async Task TestUpdateSupplierWithException()
        {
            var actionResult = await supplierController.UpdateSupplierAsync(2, null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(8)]
        public async Task TestGetNumberOfSuppliers()
        {
            var actionResult = await supplierController.GetNumberOfSuppliersAsync();
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            int result = (int)(actionResult as OkObjectResult)!.Value!;
            Assert.IsNotNull(result);
            Assert.NotZero(result);
            Assert.That(result, Is.EqualTo(2));
        }

        [OneTimeTearDown]
        public void ClearUp()
        {
            martDbContext.Database.EnsureDeleted();
        }

        private List<SupplierDto> SupplierDtos(string com_name,string phone)
        {
            return new List<SupplierDto>
            {
                new SupplierDto()
                {
                    CompanyName = com_name,
                    Owner = "Light Yagami",
                    Address = "Tokyo",
                    PhoneNumber = phone,
                    IsActive = true
                }
            };
        }
    }
}
