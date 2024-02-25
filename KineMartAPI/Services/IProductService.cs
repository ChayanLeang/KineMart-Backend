using KineMartAPI.ModelEntities;

namespace KineMartAPI.Services
{
    public interface IProductService
    {
        Task AddProductAsync(IEnumerable<Product> products);
        Task<IEnumerable<Product>> GetProductsAsync(string order, string search, int pageNumber,int pageSize);
        Task UpdateProductAsync(int id, Product product);
        Task<int> GetNumberOfProductsAsync();
    }
}
