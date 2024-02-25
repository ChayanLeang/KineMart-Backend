using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;

namespace KineMartAPI.Services
{
    public interface IProductPropertyService
    {
        Task AddProductPropertyAsync(ImportDto importDto);
        Task<ProductProperty> GetProductPropertyByIdAsync(int id);
        Task<ProductProperty> UpdateProductPropertyAsync(int id,int qty,bool isUpdate, ProductProperty 
                                                                                      productProperty);
        Task<IEnumerable<ProductProperty>> GetProductPropertiesAsync(string order, string search,int pageNumber, 
                                                                                                  int pageSize);
        
    }
}
