using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.RepositoryImpls
{
    public class ProductRepository : RepositoryBase<Product>,IProductRepository
    {
        public ProductRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }
        public async Task<IEnumerable<Product>> FindProductsWithCategoryAsync()
        {
            return await FindAllAsync().Include(pt => pt.Category).ToListAsync();
        }
    }
}
