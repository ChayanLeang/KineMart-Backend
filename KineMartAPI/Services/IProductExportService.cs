using KineMartAPI.ModelEntities;

namespace KineMartAPI.Services
{
    public interface IProductExportService
    {
        Task AddAndUpdateProductExportAsync(ProductExport productExport,bool isUpdate);
        Task<ProductExport> GetProductExportByConditionAsync(int exportId,int productId);
        Task<IEnumerable<ProductExport>> GetProductExportsAsync();
    }
}
