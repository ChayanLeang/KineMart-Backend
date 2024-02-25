using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.RepositoryImpls
{
    public class ExportRepository : RepositoryBase<Export>,IExportRepository
    {
        public ExportRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }

        public async Task<IEnumerable<Export>> FindExportsWithUserAsync()
        {
            return await FindAllAsync().Include(et => et.ApplicationUser).ToListAsync();
        }
    }
}
