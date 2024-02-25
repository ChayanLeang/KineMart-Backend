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
    public class CategoryControllerTest
    {
        private MartDbContext martDbContext;
        private IRepositoryWrapper repositoryWrapper;
        private ICategoryService categoryService;
        private IMapper mapper;
        private CategoryController categoryController;

        [OneTimeSetUp]
        public void SetUp()
        {
            mapper = MapperConfigInit.GetMapper();
            martDbContext = new MartDbContext(DbContextInit.DbContextOptions());
            repositoryWrapper = new RepositoryWrapper(martDbContext);
            categoryService = new CategoryService(repositoryWrapper);
            categoryController = new CategoryController(mapper, categoryService, 
                                          new NullLogger<CategoryController>());
            martDbContext.Database.EnsureCreated();
            SeedDatabase.SeedDatabaseOfCategory(martDbContext);
            martDbContext.SaveChanges();
        }

        [Test,Order(1)]
        public async Task TestAddCategoryWithNoException()
        {
            var actionResult = await categoryController.AddCategoryAsync(CategoryDtos("Drink"));
            Assert.That(actionResult, Is.TypeOf<OkResult>());

        }

        [Test, Order(2)]
        public async Task TestAddCategoryWithException()
        {
            var actionResult = await categoryController.AddCategoryAsync(null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());

        }

        [Test, Order(3)]
        public async Task TestGetCategories_WithNoOrderBy_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await categoryController.GetCategoriesAsync(null!, null!, 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Category>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test, Order(4)]
        public async Task TestGetCategories_WithOrderByName_WithNoSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await categoryController.GetCategoriesAsync("name", null!, 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Category>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test, Order(5)]
        public async Task TestGetCategories_WithOrderById_WithSearch_WithPageNumber_WithPageSize()
        {
            var actionResult = await categoryController.GetCategoriesAsync("id", "F", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (actionResult as OkObjectResult)!.Value as IEnumerable<Category>;
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test, Order(6)]
        public async Task TestUpdateCategoryWithNoException()
        {
            var actionResult = await categoryController.UpdateCategoryAsync(1, CategoryDtos("Weapon").First());
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public async Task TestUpdateCategoryWithException()
        {
            var actionResult = await categoryController.UpdateCategoryAsync(2, null!);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(8)]
        public async Task TestGetNumberOfCategories()
        {
            var actionResult = await categoryController.GetNumberOfCategoriesAsync();
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var result = (int)(actionResult as OkObjectResult)!.Value!;
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(2));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            martDbContext.Database.EnsureDeleted();
        }

        private List<CategoryDto> CategoryDtos(string cate_name)
        {
            return new List<CategoryDto>
            {
                new CategoryDto()
                {
                    CategoryName = cate_name,
                    IsActive = true
                }
            };
        }
    }
}
