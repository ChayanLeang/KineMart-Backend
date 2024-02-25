using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;
using KineMartAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace KineMartAPI.ServiceImpls
{
    public class ImportService : IImportService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IProductImportService _productImportService;
        public ImportService(IRepositoryWrapper repositoryWrapper, IProductImportService productImportService)
        {
            _repositoryWrapper = repositoryWrapper;
            _productImportService = productImportService;
        }
        public async Task AddImportAsync(Import import)
        {
            await _repositoryWrapper.ImportRepository.SaveAsync(import,false);
        }

        public async Task<IEnumerable<ImportViewModel>> GetImportByDateAsync(DateFilter dateFilter)
        {
            Check(dateFilter);
            var imports = await _repositoryWrapper.ImportRepository.FindImportsWithUserAsync();
            var importViewModels = new List<ImportViewModel>();
            var productImports = await GetProductImports();
            foreach (var it in imports.Where(it => string.Compare(it.Date, dateFilter.StartDate) >= 0 && 
                                                   string.Compare(it.Date, dateFilter.EndDate) <= 0 ))
            {
                var importRecords = new List<ImportRecordViewModel>();
                foreach (var pts in productImports.Where(pt=>pt.ImportId==it.ImportId).GroupBy(pt=>pt.Supplier
                                                                                .CompanyName + "," + pt.Date))
                {
                    var subRecords = new List<SubRecordViewModel>();
                    foreach(var pt in pts)
                    {
                        subRecords.Add(new SubRecordViewModel(pt.Product.ProductName, pt.Cost,pt.Price,pt.Qty,
                                                                                                  pt.Remain));
                    }
                    var arr = pts.Key.Split(",");
                    importRecords.Add(new ImportRecordViewModel(arr[0], arr[1], subRecords));
                }
                importViewModels.Add(new ImportViewModel(it.ImportId,it.ApplicationUser.UserName!,it.Date, importRecords));
            }
            return importViewModels;
        }

        public async Task<Import> GetLastImportAsync()
        {
            return await _repositoryWrapper.ImportRepository.FindLastAsync(it=>it.ImportId);
        }

        public async Task<double> GetTotalExpenseAsync()
        {
            var productImports = await GetProductImports();
            return productImports.Select(pt => pt.Amount).Sum();
        }

        private async Task<IEnumerable<ProductImport>> GetProductImports()
        {
            return await _productImportService.GetProductImportsAsync();
        }

        private static void Check(DateFilter dateFilter)
        {
            //var RegEx = new GeneratedRegexAttribute("^[0-9]{4}\\-[0-9]{2}\\-[0-9]{2}$");
            if (!Regex.IsMatch(dateFilter.StartDate, "^[0-9]{4}\\-[0-9]{2}\\-[0-9]{2}$"))
            {
                throw new ExceptionBase("StartDate");
            }

            if (!Regex.IsMatch(dateFilter.EndDate, "^[0-9]{4}\\-[0-9]{2}\\-[0-9]{2}$"))
            {
                throw new ExceptionBase("EndDate");
            }

            if (string.Compare(dateFilter.StartDate,dateFilter.EndDate)>0)
            {
                throw new ExceptionBase("InvalidDate");
            }
        }
    }
}
