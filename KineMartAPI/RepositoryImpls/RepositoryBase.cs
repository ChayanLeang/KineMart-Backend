
using KineMartAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KineMartAPI.RepositoryImpls
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T:class
    {
        private readonly MartDbContext _martDbContext;
        public RepositoryBase(MartDbContext martDbContext)
        {
            _martDbContext = martDbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _martDbContext.Set<T>().CountAsync();
        }

        public IQueryable<T> FindAllAsync()
        {
            return _martDbContext.Set<T>().AsNoTracking();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _martDbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FindLastAsync(Expression<Func<T, int>> expression)
        {
            return await _martDbContext.Set<T>().OrderBy(expression).LastOrDefaultAsync();
        }
        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _martDbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task SaveManyAsync(IEnumerable<T> objs)
        {
            _martDbContext.Set<T>().AddRange(objs);
            await _martDbContext.SaveChangesAsync();
        }

        public async Task SaveAsync(T obj,bool isUpdate)
        {
            if (isUpdate)
            {
                _martDbContext.Set<T>().Update(obj);
            }
            else
            {
                _martDbContext.Set<T>().Add(obj);
            }
            await _martDbContext.SaveChangesAsync();
        }
    }
}
