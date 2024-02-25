using KineMartAPI.ModelEntities;

namespace KineMartAPI.Services
{
    public interface IProductImportService
    {
        Task AddProductImportAsync(ProductImport productImport);
        Task<IEnumerable<ProductImport>> GetProductImportsAsync();
    }
}
