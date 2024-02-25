using KineMartAPI.ModelEntities;

namespace KineMartAPI.Repositories
{
    public interface IExportRepository : IRepositoryBase<Export>
    {
        Task<IEnumerable<Export>> FindExportsWithUserAsync();
    }
}
