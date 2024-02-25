using System.ComponentModel.DataAnnotations;

namespace KineMartAPI.ModelDtos
{
    public class RoleDto
    {
        [Required(ErrorMessage = "Role must not be blank or null")]
        public string Role { get; set; } = null!;
    }
}
