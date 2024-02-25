
using KineMartAPI.Exceptions;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using KineMartAPI.Services;
using KineMartAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KineMartAPI.ServiceImpls
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthenticationService(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> 
                                     roleManager, IConfiguration configuration, TokenValidationParameters
                                     tokenValidationParameters, IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<IdentityResult> AddRoleAsync(RoleDto roleDto)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleDto.Role));
        }

        public async Task<IdentityResult> AddUserAsync(RegisterDto registerDto)
        {
            var userExist = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExist != null && await _userManager.CheckPasswordAsync(userExist,registerDto.Password))
            {
                throw new UniqueException("User");
            }

            var newUser = new ApplicationUser()
            {
                UserName = registerDto.FullName,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = registerDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser,registerDto.Password);
            if (result.Succeeded)
            {
                await _userManager.SetLockoutEnabledAsync(newUser, false);
                await _userManager.AddToRoleAsync(newUser, registerDto.Role);
            }
            return result;
        }

        public async Task<int> GetNumberOfUsers()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task<IEnumerable<IdentityRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var userExist = await _userManager.FindByIdAsync(userId);
            if (userExist == null)
            {
                throw new NotFoundException(userId, "User");
            }
            return userExist;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ResponseDto> LogIn(LogInDto logInDto)
        {
            var userExist = await _userManager.FindByEmailAsync(logInDto.Email);
            if(userExist!=null && await _userManager.CheckPasswordAsync(userExist,logInDto.Password))
            {
                return await GeneratedToken(userExist, "");
            }
            return null!;
        }

        public async Task<ResponseDto> RefreshToken(TokenRequestDto tokenRequestDto)
        {
            try
            {
                var result = await VerifyAndGenerateTokenasyn(tokenRequestDto);
                if (result == null)
                {
                    throw new Exception("Invalid Token");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> UpdateUserAsync(string userId, RegisterDto registerDto)
        {
            var currentUser = await GetUserByIdAsync(userId);
            currentUser.UserName = registerDto.FullName;
            currentUser.PhoneNumber = registerDto.PhoneNumber;
            var result = await _userManager.UpdateAsync(currentUser);
            if (result.Succeeded)
            {
                var currentRole = await _userManager.GetRolesAsync(currentUser);
                await _userManager.SetLockoutEnabledAsync(currentUser, registerDto.isLockoutEnable);
                await _userManager.RemoveFromRoleAsync(currentUser, currentRole.First());
                await _userManager.AddToRoleAsync(currentUser, registerDto.Role);
            }
            return result;
        }

        private async Task<ResponseDto> GeneratedToken(ApplicationUser user, string existingrefreshToken)
        {
            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Sub,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            //Add User Roles
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!));
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: authClaim,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshToken();
            if (string.IsNullOrEmpty(existingrefreshToken))
            {
                refreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsRevoked = false,
                    UserId = user.Id,
                    DateAdd = DateTime.UtcNow,
                    DateExpire = DateTime.UtcNow.AddDays(1),
                    Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
                };
                await _refreshTokenService.AddRefreshTokenAsync(refreshToken);
            }

            var response = new ResponseDto()
            {
                Token = jwtToken,
                RefreshToken = (string.IsNullOrEmpty(existingrefreshToken)) ? refreshToken.Token :
                                existingrefreshToken,
                ExpiredAt = token.ValidTo,
                User = new UserViewModel(user.Id, user.UserName!, user.Email!, user.PhoneNumber!, user.LockoutEnabled,
                                  userRoles.First())
            };
            return response;
        }

        private async Task<ResponseDto> VerifyAndGenerateTokenasyn(TokenRequestDto tokenRequest)
        {
            var jwttokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //Check Jwt Token format
                var tokenInverification = jwttokenHandler.ValidateToken(tokenRequest.Token,
                                            _tokenValidationParameters, out var validatedToken);

                //Check Encrption Algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                StringComparison.InvariantCultureIgnoreCase);

                    if (result == false) return null!;

                }

                //Check Validate Expiry Date
                var utcExpiryDate = long.Parse(tokenInverification.Claims.FirstOrDefault(x => x.Type ==
                                               JwtRegisteredClaimNames.Exp)!.Value);

                var expiryDate = UnixTimeStampToDateTimeInUtc(utcExpiryDate);
                if (expiryDate > DateTime.UtcNow) throw new Exception("Token has not expiried yet!");

                //Refresh Token exist in Db
                var dbRefreshToken = await _refreshTokenService.GetRefreshTokenByToken(tokenRequest.Token);
                if (dbRefreshToken == null)
                {
                    throw new Exception("Refresh Token does not exist in db");
                }
                else
                {
                    //Check Valide Id
                    var jti = tokenInverification.Claims.FirstOrDefault(x => x.Type ==
                              JwtRegisteredClaimNames.Jti)!.Value;

                    if (dbRefreshToken.JwtId != jti)
                    {
                        throw new Exception("Token does not match");
                    }

                    //Check Refresh Token Expiration
                    if (dbRefreshToken.DateExpire <= DateTime.UtcNow)
                    {
                        throw new Exception("Your Refresh Token has expired");
                    }

                    //Check Refresh Token Revoked
                    if (dbRefreshToken.IsRevoked)
                    {
                        throw new Exception("Refresh Token is revoked");
                    }

                    //Generate New Token (With Existing Refresh Token)
                    var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken.UserId);
                    return await GeneratedToken(dbUserData!, tokenRequest.RefreshToken); ;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                var dbRefreshToken =await _refreshTokenService.GetRefreshTokenByToken(tokenRequest.RefreshToken);

                //Generate New Token (With Existing Refresh Token)
                var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken!.UserId);
                return await GeneratedToken(dbUserData!, tokenRequest.RefreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private DateTime UnixTimeStampToDateTimeInUtc(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp);
            return dateTimeVal;
        }
    }
}
