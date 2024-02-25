using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.RepositoryImpls
{
    public class SupplierRepository : RepositoryBase<Supplier>,ISupplierRepository
    {
        public SupplierRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }

        public async Task<IEnumerable<Supplier>> FindSuppliersAsync()
        {
            return await FindAllAsync().ToListAsync();
        }
    }
}
