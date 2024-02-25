using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;

namespace KineMartAPI.RepositoryImpls
{
    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(MartDbContext martDbContext) : base(martDbContext)
        {
        }

    }
}
