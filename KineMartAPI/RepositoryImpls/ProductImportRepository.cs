using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.RepositoryImpls
{
    public class ProductImportRepository : RepositoryBase<ProductImport>,IProductImportRepository
    {
        public ProductImportRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }

        public async Task<IEnumerable<ProductImport>> FindProuductImportsWithSupplierAndProductAsync()
        {
            return await FindAllAsync().Include(pt => pt.Supplier).Include(pt=>pt.Product).ToListAsync();
        }
    }
}
