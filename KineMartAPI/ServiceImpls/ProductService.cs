using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;
using Microsoft.IdentityModel.Tokens;

namespace KineMartAPI.ServiceImpls
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ProductService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task AddProductAsync(IEnumerable<Product> products)
        {
            foreach(var pt in products)
            {
                await CheckAsync(pt);
            }
            await _repositoryWrapper.ProductRepository.SaveManyAsync(products);
        }

        public async Task<int> GetNumberOfProductsAsync()
        {
            return await _repositoryWrapper.ProductRepository.CountAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string order,string search,int pageNumber,
                                                                                              int pageSize)
        {
            var products = await _repositoryWrapper.ProductRepository.FindProductsWithCategoryAsync();
            if (!order.IsNullOrEmpty())
            {
                switch (order)
                {
                    case "name":
                        products = products.OrderBy(pt => pt.ProductName);
                        break;

                    case "id":
                        products = products.OrderBy(pt => pt.ProductId);
                        break;
                }
            }

            if (!search.IsNullOrEmpty())
            {
                products = products.Where(pt => pt.ProductName.Contains(search, StringComparison
                                                                         .CurrentCultureIgnoreCase));
            }

            return PaginatedList<Product>.Create(products, pageNumber, pageSize);
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            var currentProduct = await GetProductByIdAsync(id);
            currentProduct.CategoryId = product.CategoryId;
            currentProduct.ProductName = product.ProductName;
            currentProduct.IsActive = product.IsActive;
            await CheckAsync(currentProduct);
            await _repositoryWrapper.ProductRepository.SaveAsync(currentProduct,true);
        }

        private async Task<Product> GetProductByIdAsync(int id)
        {
            var productExist = await _repositoryWrapper.ProductRepository.FindByIdAsync(id);
            if (productExist == null)
            {
                throw new NotFoundException(id.ToString(),"Product");
            }
            return productExist;
        }

        private async Task CheckAsync(Product product)
        {
            var productExist = await _repositoryWrapper.ProductRepository.FindByConditionAsync(pd=>pd.ProductName
                                     .Trim().ToLower().Equals(product.ProductName.Trim().ToLower()) 
                                     && pd.ProductId!=product.ProductId);
            if (productExist!=null)
            {
                throw new UniqueException($"ProductName ({productExist.ProductName})");
            }
        }
    }
}
