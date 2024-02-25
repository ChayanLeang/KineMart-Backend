using System.ComponentModel.DataAnnotations;

namespace KineMartAPI.ModelDtos
{
    public class SupplierDto
    {
        [Required(ErrorMessage = "Owner must not be null or blank")]
        public string Owner { get; set; } = null!;

        [Required(ErrorMessage = "CompanyName must not be null or blank")]
        public string CompanyName { get; set; } = null!;

        [Required(ErrorMessage = "PhoneNumber must not be null or blank")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Address must not be null or blank")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "IsActive must not be null or blank")]
        public bool IsActive { get; set; }
    }
}
