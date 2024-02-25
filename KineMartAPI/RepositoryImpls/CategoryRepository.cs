using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.RepositoryImpls
{
    public class CategoryRepository : RepositoryBase<Category>,ICategoryRepository
    {
        public CategoryRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }

        public async Task<IEnumerable<Category>> FindCategoriesAsync()
        {
            return await FindAllAsync().ToListAsync();
        }
    }
}
