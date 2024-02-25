using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;
using Microsoft.IdentityModel.Tokens;

namespace KineMartAPI.ServiceImpls
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public CategoryService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task AddCategoryAsync(IEnumerable<Category> categories)
        {
            foreach(var cy in categories)
            {
                await CheckAsync(cy);
            }
            await _repositoryWrapper.CategoryRepository.SaveManyAsync(categories);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(string order,string search,int pageNumber
                                                                                               ,int pageSize)
        {
            var categories = await _repositoryWrapper.CategoryRepository.FindCategoriesAsync();

            if (!order.IsNullOrEmpty())
            {
                switch (order)
                {
                    case "name":
                        categories = categories.OrderBy(cy => cy.CategoryName);
                        break;
                    case "id":
                        categories = categories.OrderBy(cy => cy.CategoryId);
                        break;
                }
                
            }

            if (!search.IsNullOrEmpty())
            {
                categories = categories.Where(cy => cy.CategoryName.Contains(search, StringComparison
                                                                          .CurrentCultureIgnoreCase));
            }

            return PaginatedList<Category>.Create(categories, pageNumber, pageSize);
        }

        private async Task<Category> GetCategoryByIdAsync(int id)
        {
            var categoryExist = await _repositoryWrapper.CategoryRepository.FindByIdAsync(id);
            if (categoryExist == null)
            {
                throw new NotFoundException(id.ToString(), "Category");
            }
            return categoryExist;
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            var currentCategory = await GetCategoryByIdAsync(id);
            currentCategory.CategoryName = category.CategoryName;
            currentCategory.IsActive = category.IsActive;
            await CheckAsync(currentCategory);
            await _repositoryWrapper.CategoryRepository.SaveAsync(currentCategory,true);
        }

        public async Task<int> GetNumberOfCategoriesAsync()
        {
            return await _repositoryWrapper.CategoryRepository.CountAsync();
        }

        private async Task CheckAsync(Category category)
        {
            var categoryExist = await _repositoryWrapper.CategoryRepository.FindByConditionAsync(cy =>
                                          cy.CategoryName.Trim().ToLower().Equals(category.CategoryName.Trim()
                                          .ToLower()) && cy.CategoryId != category.CategoryId);
            if (categoryExist != null)
            {
                throw new UniqueException($"CategoryName ({categoryExist.CategoryName})");
            }

        }
    }
}
