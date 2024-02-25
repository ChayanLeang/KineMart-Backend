using KineMartAPI.ModelEntities;

namespace KineMartAPI.Repositories
{
    public interface IProductPropertyRepository : IRepositoryBase<ProductProperty>
    {
        Task<IEnumerable<ProductProperty>> FindProductPropertiesWithProductAsync();
    }
}
