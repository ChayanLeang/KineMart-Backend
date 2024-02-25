using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.RepositoryImpls
{
    public class ImportRepository : RepositoryBase<Import>,IImportRepository
    {
        public ImportRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }

        public async Task<IEnumerable<Import>> FindImportsWithUserAsync()
        {
            return await FindAllAsync().Include(it => it.ApplicationUser).ToListAsync();
        }
    }
}
