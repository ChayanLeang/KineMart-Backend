using KineMartAPI.Exceptions;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using KineMartAPI.Services;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace KineMartAPI.ServiceImpls
{
    public class SaleService : ISaleService
    {
        private readonly IExportService _exportService;
        private readonly IProductExportService _productExportService;
        private readonly IProductPropertyService _productPropertyService;
        public SaleService(IExportService exportService, IProductExportService productExportService,
                           IProductPropertyService productPropertyService)
        {
            _exportService = exportService;
            _productExportService = productExportService;
            _productPropertyService = productPropertyService;
        }
        public async Task SellAsync(SaleDto saleDto)
        {
            await CheckAsync(saleDto.Products);
            var lastExport = await _exportService.GetLastExportAsync();
            if (lastExport != null)
            {
                if (lastExport.Date.Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    await Add(lastExport.ExportId, saleDto.Products);
                }
                else
                {
                    var export = await AddToExport(saleDto.UserId);
                    await Add(export.ExportId, saleDto.Products);
                }
            }
            else
            {
                var export = await AddToExport(saleDto.UserId);
                await Add(export.ExportId, saleDto.Products);
            }
        }

        private async Task Add(int exportId,List<ProductSaleDto> productSaleDtos)
        {
            foreach (var pd in productSaleDtos)
            {
                var productPro = await _productPropertyService.GetProductPropertyByIdAsync(pd.ProductProId);
                await AddToProductExport(exportId, pd.Qty,productPro);
            }
        }
        private async Task AddToProductExport(int exportId,int qty,ProductProperty productPro)
        {
            var productExportExist = await _productExportService.GetProductExportByConditionAsync(exportId, 
                                                                                     productPro.ProductId);
            if(productExportExist!=null)
            {
                productExportExist.Qty += qty;
                productExportExist.Amount = productExportExist.Price * productExportExist.Qty;
                await _productExportService.AddAndUpdateProductExportAsync(productExportExist, true);
            }
            else
            {
                var productExport = new ProductExport()
                {
                    ExportId = exportId,
                    ProductId = productPro.ProductId,
                    Price = productPro.Price,
                    Qty = qty,
                    Amount = productPro.Price * qty
                };
                await _productExportService.AddAndUpdateProductExportAsync(productExport,false);
            }
            await _productPropertyService.UpdateProductPropertyAsync(productPro.ProductProId, qty, false, null!);
        }
        private async Task<Export> AddToExport(string userId)
        {
            var export = new Export()
            {
                UserId= userId,
                Date=DateTime.Now.ToString("yyyy-MM-dd")
            };
            await _exportService.AddExportAsync(export);
            return export;
        }
        private async Task CheckAsync(List<ProductSaleDto> productSaleDtos)
        {
            foreach (var pd in productSaleDtos)
            {
                var productPro = await _productPropertyService.GetProductPropertyByIdAsync(pd.ProductProId);
                if (productPro.Qty < pd.Qty)
                {
                    throw new ExceptionBase(productPro.Product.ProductName);
                }
            }
        }
    }
}
