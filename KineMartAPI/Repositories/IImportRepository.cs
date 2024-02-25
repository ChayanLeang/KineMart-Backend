using KineMartAPI.ModelEntities;

namespace KineMartAPI.Repositories
{
    public interface IImportRepository : IRepositoryBase<Import>
    {
        public Task<IEnumerable<Import>> FindImportsWithUserAsync();
    }
}
