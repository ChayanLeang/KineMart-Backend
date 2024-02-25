using KineMartAPI.ModelEntities;

namespace KineMartAPI.Repositories
{
    public interface IProductExportRepository : IRepositoryBase<ProductExport>
    {
        Task<IEnumerable<ProductExport>> FindProductExportsWithExportAndProductAsync();
    }
}
