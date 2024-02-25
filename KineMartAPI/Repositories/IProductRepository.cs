using KineMartAPI.ModelEntities;

namespace KineMartAPI.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> FindProductsWithCategoryAsync();
    }
}
