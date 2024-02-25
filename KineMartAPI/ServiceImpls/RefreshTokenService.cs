using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;

namespace KineMartAPI.ServiceImpls
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public RefreshTokenService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _repositoryWrapper.RefreshTokenRepository.SaveAsync(refreshToken, false);
        }

        public async Task<RefreshToken> GetRefreshTokenByToken(string token)
        {
            var refreshTokenExist = await _repositoryWrapper.RefreshTokenRepository.FindByConditionAsync(rn => 
                                                                                      rn.Token.Equals(token));
            if (refreshTokenExist == null)
            {
                throw new NotFoundException(token,"RefreshToken");
            }
            return refreshTokenExist;
        }
    }
}
