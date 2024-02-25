using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using Microsoft.AspNetCore.Identity;

namespace KineMartAPI.Services
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> AddUserAsync(RegisterDto registerDto);
        Task<IdentityResult> UpdateUserAsync(string userId,RegisterDto registerDto);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IdentityResult> AddRoleAsync(RoleDto roleDto);
        Task<IEnumerable<IdentityRole>> GetRolesAsync();
        Task<int> GetNumberOfUsers();
        Task<ResponseDto> LogIn(LogInDto logInDto);
        Task<ResponseDto> RefreshToken(TokenRequestDto tokenRequestDto);
    }
}
