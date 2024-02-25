using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI.ServiceImpls
{
    public class LogService : ILogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public LogService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<IEnumerable<Log>> GetLogsAsync()
        {
            return await _repositoryWrapper.LogRepository.FindAllAsync().ToListAsync();
        }
    }
}
