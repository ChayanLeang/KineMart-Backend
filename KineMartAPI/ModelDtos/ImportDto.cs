namespace KineMartAPI.ModelDtos
{
    public class ImportDto
    {
        public int SupplierId { get; set; }
        public string UserId { get; set; } = null!;
        public List<ProductImportDto> Products { get; set; } = null!;
    }
}
