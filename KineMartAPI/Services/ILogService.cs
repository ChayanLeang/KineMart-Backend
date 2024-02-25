using KineMartAPI.ModelEntities;

namespace KineMartAPI.Services
{
    public interface ILogService
    {
        Task<IEnumerable<Log>> GetLogsAsync();
    }
}
