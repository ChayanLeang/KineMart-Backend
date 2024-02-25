using KineMartAPI.ModelEntities;
using KineMartAPI.ViewModels;

namespace KineMartAPI.Services
{
    public interface IImportService
    {
        Task AddImportAsync(Import import);
        Task<Import> GetLastImportAsync();
        Task<IEnumerable<ImportViewModel>> GetImportByDateAsync(DateFilter dateFilter);
        Task<double> GetTotalExpenseAsync();
    }
}
