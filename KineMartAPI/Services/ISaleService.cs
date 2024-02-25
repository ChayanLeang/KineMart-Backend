using KineMartAPI.ModelDtos;

namespace KineMartAPI.Services
{
    public interface ISaleService
    {
        Task SellAsync(SaleDto saleDto);
    }
}
