using System.ComponentModel.DataAnnotations;

namespace KineMartAPI.ModelDtos
{
    public class LogInDto
    {
        [Required(ErrorMessage = "Email must not be blank or null")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password must not be blank or null")]
        public string Password { get; set; } = null!;
    }
}
