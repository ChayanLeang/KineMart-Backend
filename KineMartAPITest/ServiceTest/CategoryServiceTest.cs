using KineMartAPI;
using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;

namespace KineMartAPITest.ServiceTest
{
    public class CategoryServiceTest
    {
        private MartDbContext martDbContext;
        private ICategoryService categoryService;
        private IRepositoryWrapper repositoryWrapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            categoryService = new CategoryService(repositoryWrapper);
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfCategory(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test, Order(1)]
        public void TestAddCategoryWithNoException()
        {
            Assert.That(async () => await categoryService.AddCategoryAsync(Categories("Drink")),Throws.Nothing);
        }

        [Test, Order(2)]
        public void TestAddCategoryWithException()
        {
            Assert.That(async () => await categoryService.AddCategoryAsync(Categories("Food")), Throws.Exception
                                    .TypeOf<UniqueException>().With.Message.EqualTo
                                    ("CategoryName (Food) was already exist"));
        }

        [Test, Order(3)]
        public async Task GetAllCategories_WithNoOrderBy_WithNoSearch_WithNoPageNumber_WithNoPageSize()
        {
            var categories = await categoryService.GetCategoriesAsync(null!, null!, 0, 0);
            Assert.IsEmpty(categories);
            Assert.IsNotNull(categories);
            Assert.That(categories.Count(), Is.EqualTo(0));
        }

        [Test, Order(4)]
        public async Task GetAllCategories_WithNoOrderBy_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var categories = await categoryService.GetCategoriesAsync(null!, null!, 1, 5);
            Assert.IsNotEmpty(categories);
            Assert.IsNotNull(categories);
            Assert.That(categories.Count(), Is.EqualTo(2));
        }

        [Test, Order(5)]
        public async Task GetAllCategories_WithOrderByName_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var categories = await categoryService.GetCategoriesAsync("name", null!, 1, 5);
            Assert.IsNotEmpty(categories);
            Assert.IsNotNull(categories);
            Assert.That(categories.Count(), Is.EqualTo(2));
            var category = categories.FirstOrDefault()!;
            Assert.That(category.CategoryId, Is.EqualTo(2));
            Assert.That(category.CategoryName, Is.EqualTo("Drink"));
            Assert.That(category.IsActive, Is.EqualTo(true));
        }

        [Test, Order(6)]
        public async Task GetAllCategories_WithOrderById_WithSearch_WithPageNumber_WithPageSize()
        {
            var categories = await categoryService.GetCategoriesAsync("id","F", 1, 5);
            Assert.IsNotEmpty(categories);
            Assert.IsNotNull(categories);
            Assert.That(categories.Count(), Is.EqualTo(1));
            var category = categories.FirstOrDefault()!;
            Assert.That(category.CategoryId, Is.EqualTo(1));
            Assert.That(category.CategoryName, Is.EqualTo("Food"));
            Assert.That(category.IsActive, Is.EqualTo(true));
        }

        [Test, Order(7)]
        public void TestUpdateCategoryWithNoException()
        {
            Assert.That(async () => await categoryService.UpdateCategoryAsync(1, Categories("Electronic").First()), 
                                                                                              Throws.Nothing);
        }

        [Test, Order(8)]
        public void TestUpdateCategoryWithException1()
        {
            Assert.That(async () => await categoryService.UpdateCategoryAsync(3, Categories("Electronic").First()),
                                    Throws.Exception.TypeOf<NotFoundException>().With.Message.EqualTo
                                    ("Category with id = 3 didn't find"));
        }

        [Test, Order(9)]
        public void TestUpdateCategoryWithException2()
        {
            Assert.That(async () => await categoryService.UpdateCategoryAsync(2, Categories("Electronic").First()), 
                                    Throws.Exception.TypeOf<UniqueException>().With.Message.EqualTo
                                    ("CategoryName (Electronic) was already exist"));
        }

        [Test, Order(10)]
        public async Task TestGetNumberOfCategories()
        {
            int numbers = await categoryService.GetNumberOfCategoriesAsync();
            Assert.IsNotNull(numbers);
            Assert.NotZero(numbers);
            Assert.That(numbers, Is.EqualTo(2));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }

        private List<Category> Categories(string name1)
        {
            return new List<Category>
            {
                new Category()
                {
                    CategoryName=name1,
                    IsActive=true
                },
            };
        }
    }
}
