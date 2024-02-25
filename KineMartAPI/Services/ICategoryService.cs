using KineMartAPI.ModelEntities;

namespace KineMartAPI.Services
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(IEnumerable<Category> category);
        Task<IEnumerable<Category>> GetCategoriesAsync(string prder,string search,int pageNumber, int pageSize);
        Task UpdateCategoryAsync(int id, Category category);
        Task<int> GetNumberOfCategoriesAsync();
    }
}
