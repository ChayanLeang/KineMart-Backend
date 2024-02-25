namespace KineMartAPI.ModelDtos
{
    public class SaleDto
    {
        public string UserId { get; set; } = null!;
        public List<ProductSaleDto> Products { get; set; } = null!;
    }
}
