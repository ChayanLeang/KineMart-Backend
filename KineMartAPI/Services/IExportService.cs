using KineMartAPI.ModelEntities;
using KineMartAPI.ViewModels;

namespace KineMartAPI.Services
{
    public interface IExportService
    {
        Task AddExportAsync(Export export);
        Task<Export> GetLastExportAsync();
        Task<IEnumerable<ExportViewModel>> GetExportByDateAsync(DateFilter dateFilter);
        Task<double> GetTotalRevenusAsync();
    }
}
