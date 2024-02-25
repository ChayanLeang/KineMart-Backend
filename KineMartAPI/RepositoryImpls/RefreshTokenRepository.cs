using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;

namespace KineMartAPI.RepositoryImpls
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }
    }
}
