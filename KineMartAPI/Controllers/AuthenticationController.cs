using KineMartAPI.ModelDtos;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KineMartAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(ModelState.IsValid && registerDto != null)
            {
                var result=await _authenticationService.AddUserAsync(registerDto);
                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> Update(string userId,RegisterDto registerDto)
        {
            if (ModelState.IsValid && registerDto != null)
            {
                var result = await _authenticationService.UpdateUserAsync(userId, registerDto);
                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logIn")]
        public async Task<IActionResult> LogIn(LogInDto logInDto)
        {
            if(ModelState.IsValid && logInDto != null)
            {
                var response = await _authenticationService.LogIn(logInDto);
                if (response != null)
                {
                    return Ok(response);
                }
                return Unauthorized("Wrong Email or Password");
            }
            return BadRequest(ModelState);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRequestDto tokenRequest)
        {
            return Ok(await _authenticationService.RefreshToken(tokenRequest));
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(RoleDto roleDto)
        {
            if(ModelState.IsValid && roleDto != null)
            {
                var result = await _authenticationService.AddRoleAsync(roleDto);
                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _authenticationService.GetUsersAsync());
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _authenticationService.GetRolesAsync());
        }

        [HttpGet("number-of-users")]
        public async Task<IActionResult> Count()
        {
            return Ok(await _authenticationService.GetNumberOfUsers());
        }
    }
}
