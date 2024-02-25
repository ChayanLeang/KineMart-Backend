using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.RepositoryImpls
{
    public class ProductPropertyRepository : RepositoryBase<ProductProperty>,IProductPropertyRepository
    {
        public ProductPropertyRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }

        public async Task<IEnumerable<ProductProperty>> FindProductPropertiesWithProductAsync()
        {
            return await FindAllAsync().Include(py => py.Product).ToListAsync();
        }
    }
}
