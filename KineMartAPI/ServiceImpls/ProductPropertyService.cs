using KineMartAPI.Exceptions;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;
using Microsoft.IdentityModel.Tokens;

namespace KineMartAPI.ServiceImpls
{
    public class ProductPropertyService : IProductPropertyService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IImportService _importService;
        private readonly IProductImportService _productImportService;
        public ProductPropertyService(IRepositoryWrapper repositoryWrapper, IImportService importService
                                      , IProductImportService productImportService)
        {
            _repositoryWrapper = repositoryWrapper;
            _importService = importService;
            _productImportService = productImportService;
        }
        public async Task AddProductPropertyAsync(ImportDto importDto)
        {
            var lastImport = await _importService.GetLastImportAsync();
            if (lastImport != null)
            {
                if (lastImport.Date.Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    await Add(lastImport.ImportId, importDto);
                }
                else
                {
                    var import = AddToImport(importDto.UserId);
                    await Add(import.ImportId,importDto);
                }
            }
            else
            {
                var import = AddToImport(importDto.UserId);
                await Add(import.ImportId, importDto);
            }
        }

        public async Task<ProductProperty> UpdateProductPropertyAsync(int id,int qty,bool isUpdate, 
                                                                   ProductProperty productProperty)
        {
            var currentPro = await GetProductPropertyByIdAsync(id);
            if (isUpdate)
            {
                currentPro.Cost = productProperty.Cost;
                currentPro.Price = productProperty.Price;
                currentPro.Qty += productProperty.Qty;
            }
            else
            {
                currentPro.Qty -= qty;
            }
            await _repositoryWrapper.ProductPropertyRepository.SaveAsync(currentPro, true);
            return currentPro;
        }

        public async Task<IEnumerable<ProductProperty>> GetProductPropertiesAsync(string order,string search,
                                                                                 int pageNumber,int pageSize)
        {
            var productPropreties= await _repositoryWrapper.ProductPropertyRepository
                                                           .FindProductPropertiesWithProductAsync();

            if (!order.IsNullOrEmpty())
            {
                switch (order)
                {
                    case "id":
                        productPropreties = productPropreties.OrderBy(py => py.ProductProId);
                        break;

                    case "name":
                        productPropreties = productPropreties.OrderBy(py => py.Product.ProductName);
                        break;
                }
                
            }

            if (!search.IsNullOrEmpty())
            {
                productPropreties = productPropreties.Where(py => py.Product.ProductName.Contains(search, 
                                                             StringComparison.CurrentCultureIgnoreCase));
            }

            return PaginatedList<ProductProperty>.Create(productPropreties, pageNumber, pageSize);
        }

        public async Task<ProductProperty> GetProductPropertyByIdAsync(int id)
        {
            var productProExist = await _repositoryWrapper.ProductPropertyRepository.FindByIdAsync(id);
            if (productProExist == null)
            {
                throw new NotFoundException(id.ToString(), "ProductProperty");
            }
            return productProExist;
        }

        private async Task Add(int importId,ImportDto importDto)
        {
            foreach (var pro in importDto.Products)
            {
                var productPro = new ProductProperty()
                {
                    ProductId = pro.ProductId,
                    Cost = pro.Cost,
                    Price = pro.Price,
                    Qty = pro.Qty
                };
                var existPro = await GetProductPropertyByProId(pro.ProductId);
                if (existPro != null)
                {
                    var newPro = await UpdateProductPropertyAsync(existPro.ProductId, 0, true, productPro);
                    await AddToProductImport(importDto.SupplierId,importId,productPro,newPro.Qty-productPro.Qty);
                }
                else
                {
                    await _repositoryWrapper.ProductPropertyRepository.SaveAsync(productPro,false);
                    await AddToProductImport(importDto.SupplierId, importId, productPro,0);
                }
                
            }
        }

        private async Task<ProductProperty> GetProductPropertyByProId(int id)
        {
            return await _repositoryWrapper.ProductPropertyRepository.FindByConditionAsync(py => py.ProductId == 
                                                                                                            id);
        }
        private Import AddToImport(string userId)
        {
            var import = new Import()
            {
                UserId=userId,
                Date = DateTime.Now.ToString("yyyy-MM-dd")
            };
            _importService.AddImportAsync(import);
            return import;
        }

        private async Task AddToProductImport(int supplierId,int importId,ProductProperty productPro,int remain)
        {
            var productImport = new ProductImport()
            {
                ImportId = importId,
                ProductId = productPro.ProductId,
                SupplierId = supplierId,
                Cost = productPro.Cost,
                Price = productPro.Price,
                Qty = productPro.Qty,
                Amount = productPro.Cost * productPro.Qty,
                Remain = remain,
                Date=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt")
            };
            await _productImportService.AddProductImportAsync(productImport);
        }

    }
}
