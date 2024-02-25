using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.RepositoryImpls
{
    public class ProductExportRepository : RepositoryBase<ProductExport>,IProductExportRepository
    {
        public ProductExportRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }

        public async Task<IEnumerable<ProductExport>> FindProductExportsWithExportAndProductAsync()
        {
            return await FindAllAsync().Include(pt=>pt.Product).ToListAsync();
        }
    }
}
