using System.ComponentModel.DataAnnotations;

namespace KineMartAPI.ModelDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email must not be blank or null")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password must not be blank or null")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "FullName must not be blank or null")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "PhoneNumber must not be blank or null")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Role must not be blank or null")]
        public string Role { get; set; } = null!;
        public bool isLockoutEnable { get; set; }
    }
}
