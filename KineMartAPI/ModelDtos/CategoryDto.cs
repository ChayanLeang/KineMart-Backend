using System.ComponentModel.DataAnnotations;

namespace KineMartAPI.ModelDtos
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "ProductName must not be null or blank")]
        public string CategoryName { get; set; } = null!;

        [Required(ErrorMessage = "IsActive must not be null or blank")]
        public bool IsActive { get; set; }
    }
}
