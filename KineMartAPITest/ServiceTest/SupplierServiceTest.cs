using KineMartAPI;
using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;

namespace KineMartAPITest.ServiceTest
{
    public class SupplierServiceTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private ISupplierService supplierService;

        [OneTimeSetUp]
        public void Setup()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            supplierService = new SupplierService(repositoryWrapper);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfSupplier(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public void TestAddSupplierWithNoException()
        {
            Assert.That(async () => await supplierService.AddSupplierAsync(Suppliers("Era Company", "097233444")), 
                                                                                             Throws.Nothing);
        }

        [Test, Order(2)]
        public void TestAddSupplierWithException1()
        {
            Assert.That(async () => await supplierService.AddSupplierAsync(Suppliers("Era Company", "9723344")), 
                                          Throws.Exception.TypeOf<ExceptionBase>().With.Message.EqualTo
                                          ("PhoneNumber (9723344) should be has 9 digits and start from 0"));
        }

        [Test, Order(3)]
        public void TestAddSupplierWithException2()
        {
            Assert.That(async () => await supplierService.AddSupplierAsync(Suppliers("Food Company", "085445111")), 
                                          Throws.Exception.TypeOf<UniqueException>().With.Message.EqualTo
                                          ("CompanyName (Food Company) was already exist"));
        }

        [Test, Order(4)]
        public void TestAddSupplierWithException3()
        {
            Assert.That(async () => await supplierService.AddSupplierAsync(Suppliers("Li Company", "097233444")), 
                                    Throws.Exception.TypeOf<UniqueException>().With.Message.EqualTo
                                    ("PhoneNumber (097233444) was already exist"));
        }

        [Test, Order(5)]
        public async Task TestGetSuppliers_WithNoOrderBy_WithNoSearch_WithNoPageNumber_WithNoPageSize()
        {
            var suppliers = await supplierService.GetSuppliersAsync(null!, null!, 0, 0);
            Assert.IsNotNull(suppliers);
            Assert.IsEmpty(suppliers);
            Assert.That(suppliers.Count(), Is.EqualTo(0));
        }

        [Test, Order(6)]
        public async Task TestGetSuppliers_WithNoOrderBy_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var suppliers = await supplierService.GetSuppliersAsync(null!, null!, 1, 5);
            Assert.IsNotNull(suppliers);
            Assert.IsNotEmpty(suppliers);
            Assert.That(suppliers.Count(), Is.EqualTo(2));
        }

        [Test, Order(7)]
        public async Task TestGetSuppliers_WithOrderByName_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var suppliers = await supplierService.GetSuppliersAsync("name", null!, 1, 5);
            Assert.IsNotNull(suppliers);
            Assert.IsNotEmpty(suppliers);
            Assert.That(suppliers.Count(), Is.EqualTo(2));
            var supplier = suppliers.FirstOrDefault()!;
            Assert.That(supplier.SupplierId, Is.EqualTo(2));
            Assert.That(supplier.CompanyName, Is.EqualTo("Era Company"));
            Assert.That(supplier.Owner, Is.EqualTo("Akina"));
            Assert.That(supplier.PhoneNumber, Is.EqualTo("097233444"));
            Assert.That(supplier.Address, Is.EqualTo("Kyoto"));
            Assert.That(supplier.IsActive, Is.EqualTo(true));
        }

        [Test, Order(8)]
        public async Task TestGetSuppliers_WithOrderById_WithSearch_WithPageNumber_WithPageSize()
        {
            var suppliers = await supplierService.GetSuppliersAsync("id", "f", 1, 5);
            Assert.IsNotNull(suppliers);
            Assert.IsNotEmpty(suppliers);
            Assert.That(suppliers.Count(), Is.EqualTo(1));
            var supplier = suppliers.FirstOrDefault()!;
            Assert.That(supplier.SupplierId, Is.EqualTo(1));
            Assert.That(supplier.CompanyName, Is.EqualTo("Food Company"));
            Assert.That(supplier.Owner, Is.EqualTo("Light Yagami"));
            Assert.That(supplier.PhoneNumber, Is.EqualTo("098222111"));
            Assert.That(supplier.Address, Is.EqualTo("Tokyo"));
            Assert.That(supplier.IsActive, Is.EqualTo(true));
        }

        [Test, Order(9)]
        public void TestUpdateSupplierWithNoException()
        {
            Assert.That(async () => await supplierService.UpdateSupplierAsync(1, Suppliers("Boxing Company", 
                                                                "086455222").First()), Throws.Nothing);
        }

        [Test, Order(10)]
        public void TestUpdateSupplierWithException1()
        {
            Assert.That(async () => await supplierService.UpdateSupplierAsync(4, Suppliers("Boxing Company", "8645522")
                                    .First()), Throws.Exception.TypeOf<NotFoundException>().With.Message
                                    .EqualTo("Supplier with id = 4 didn't find"));
        }

        [Test, Order(11)]
        public void TestUpdateSupplierWithException2()
        {
            Assert.That(async () => await supplierService.UpdateSupplierAsync(1, Suppliers("Boxing Company", "8645522")
                                    .First()), Throws.Exception.TypeOf<ExceptionBase>().With.Message
                                    .EqualTo("PhoneNumber (8645522) should be has 9 digits and start from 0"));
        }

        [Test, Order(12)]
        public void TestUpdateSupplierWithException3()
        {
            Assert.That(async () => await supplierService.UpdateSupplierAsync(2, Suppliers("Boxing Company", 
                                          "086455223").First()), Throws.Exception.TypeOf<UniqueException>().With
                                          .Message.EqualTo("CompanyName (Boxing Company) was already exist"));
        }

        [Test, Order(13)]
        public void TestUpdateSupplierWithException4()
        {
            Assert.That(async () => await supplierService.UpdateSupplierAsync(2, Suppliers("Pi Company", "086455222")
                                    .First()), Throws.Exception.TypeOf<UniqueException>().With.Message
                                    .EqualTo("PhoneNumber (8645522) was already exist"));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }

        private List<Supplier> Suppliers(string com_name,string phone)
        {
            return new List<Supplier>
            {
                new Supplier()
                {
                    CompanyName=com_name,
                    Owner="Akina",
                    PhoneNumber=phone,
                    Address="Kyoto",
                    IsActive=true
                }
            };
        }
    }
}
