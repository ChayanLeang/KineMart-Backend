
using System.ComponentModel.DataAnnotations;

namespace KineMartAPI.ModelDtos
{
    public class ProductDto
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "ProductName must not be null or blank")]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "IsActive must not be null or blank")]
        public bool IsActive { get; set; }
    }
}
