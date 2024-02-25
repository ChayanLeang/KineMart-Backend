using KineMartAPI.ModelEntities;

namespace KineMartAPI.Services
{
    public interface IRefreshTokenService
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByToken(string token);
    }
}
