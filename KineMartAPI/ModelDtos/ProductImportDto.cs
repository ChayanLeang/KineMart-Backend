
using System.ComponentModel.DataAnnotations;

namespace KineMartAPI.ModelDtos
{
    public class ProductImportDto
    {
        public int ProductId { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
    }
}
