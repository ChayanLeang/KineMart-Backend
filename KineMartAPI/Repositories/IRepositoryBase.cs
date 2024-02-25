using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace KineMartAPI.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task SaveManyAsync(IEnumerable<T> objs);
        Task SaveAsync(T obj,bool isUpdate);
        IQueryable<T> FindAllAsync();
        Task<T> FindByIdAsync(int id);
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task<T> FindLastAsync(Expression<Func<T, int>> expression);
        Task<int> CountAsync();
    }
}
