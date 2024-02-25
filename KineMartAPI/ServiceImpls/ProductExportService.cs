using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;

namespace KineMartAPI.ServiceImpls
{
    public class ProductExportService : IProductExportService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ProductExportService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task AddAndUpdateProductExportAsync(ProductExport productExport,bool isUpdate)
        {
            if (isUpdate)
            {
                await _repositoryWrapper.ProductExportRepository.SaveAsync(productExport, true);
            }
            else
            {
                await _repositoryWrapper.ProductExportRepository.SaveAsync(productExport, false);
            }
            
        }

        public async Task<ProductExport> GetProductExportByConditionAsync(int exportId, int productId)
        {
            return await _repositoryWrapper.ProductExportRepository.FindByConditionAsync(pt => pt.ExportId == 
                                                                 exportId && pt.ProductId == productId);
        }

        public async Task<IEnumerable<ProductExport>> GetProductExportsAsync()
        {
            return await _repositoryWrapper.ProductExportRepository.FindProductExportsWithExportAndProductAsync();
        }
    }
}
