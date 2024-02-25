using KineMartAPI.ModelEntities;

namespace KineMartAPI.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> FindCategoriesAsync();
    }
}
