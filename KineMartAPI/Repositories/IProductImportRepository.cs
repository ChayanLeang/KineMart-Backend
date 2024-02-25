using KineMartAPI.ModelEntities;

namespace KineMartAPI.Repositories
{
    public interface IProductImportRepository : IRepositoryBase<ProductImport>
    {
        Task<IEnumerable<ProductImport>> FindProuductImportsWithSupplierAndProductAsync();
    }
}
