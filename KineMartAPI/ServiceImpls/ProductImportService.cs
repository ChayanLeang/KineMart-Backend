using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;

namespace KineMartAPI.ServiceImpls
{
    public class ProductImportService : IProductImportService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ProductImportService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task AddProductImportAsync(ProductImport productImport)
        {
            await _repositoryWrapper.ProductImportRepository.SaveAsync(productImport,false);
        }

        public async Task<IEnumerable<ProductImport>> GetProductImportsAsync()
        {
            return await _repositoryWrapper.ProductImportRepository
                                           .FindProuductImportsWithSupplierAndProductAsync();
        }
    }
}
