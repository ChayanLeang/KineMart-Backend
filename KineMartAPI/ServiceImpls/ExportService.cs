using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;
using KineMartAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace KineMartAPI.ServiceImpls
{
    public class ExportService : IExportService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IProductExportService _productExportService;
        public ExportService(IRepositoryWrapper repositoryWrapper,IProductExportService productExportService)
        {
            _repositoryWrapper = repositoryWrapper;
            _productExportService = productExportService;
        }
        public async Task AddExportAsync(Export export)
        {
            await _repositoryWrapper.ExportRepository.SaveAsync(export,false);
        }

        public async Task<IEnumerable<ExportViewModel>> GetExportByDateAsync(DateFilter dateFilter)
        {
            Check(dateFilter);
            var exports = await _repositoryWrapper.ExportRepository.FindExportsWithUserAsync();
            var productExports = await GetProductExports();
            var exportViewModels = new List<ExportViewModel>();
            foreach(var et in exports.Where(et => string.Compare(et.Date,dateFilter.StartDate)>=0 && 
                                                  string.Compare(et.Date, dateFilter.EndDate) <= 0))
            {
                var exportRecords = new List<ExportRecordViewModel>();
                foreach(var pt in productExports.Where(pt=>pt.ExportId==et.ExportId))
                {
                    exportRecords.Add(new ExportRecordViewModel(pt.Product.ProductName,pt.Price,pt.Qty));
                }
                exportViewModels.Add(new ExportViewModel(et.ExportId,et.ApplicationUser.UserName!,et.Date,
                                                                                          exportRecords));
            }
            return exportViewModels;
        }

        public async Task<Export> GetLastExportAsync()
        {
            return await _repositoryWrapper.ExportRepository.FindLastAsync(et=>et.ExportId);
        }

        public async Task<double> GetTotalRevenusAsync()
        {
            var productExports = await GetProductExports();
            return productExports.Select(pt => pt.Amount).Sum();
        }

        private async Task<IEnumerable<ProductExport>> GetProductExports()
        {
            return await _productExportService.GetProductExportsAsync();
        }

        private static void Check(DateFilter dateFilter)
        {
            //var regEx = new GeneratedRegexAttribute("^[0-9]{4}\\-[0-9]{2}\\-[0-9]{2}$");
            if (!Regex.IsMatch(dateFilter.StartDate, "^[0-9]{4}\\-[0-9]{2}\\-[0-9]{2}$"))
            {
                throw new ExceptionBase("StartDate");
            }

            if (!Regex.IsMatch(dateFilter.EndDate, "^[0-9]{4}\\-[0-9]{2}\\-[0-9]{2}$"))
            {
                throw new ExceptionBase("EndDate");
            }

            if (string.Compare(dateFilter.StartDate, dateFilter.EndDate) > 0)
            {
                throw new ExceptionBase("InvalidDate");
            }
        }
    }
}
