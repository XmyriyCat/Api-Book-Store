using System.Linq.Expressions;
using DLL.Errors;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext dbContext;

        protected GenericRepository(DbContext context)
        {
            dbContext = context;
        }

        public IQueryable<T> GetAll()
        {
            return dbContext.Set<T>().AsNoTracking();
        }
        
        public async Task<T> FindAsync(int id)
        {
            var item = await dbContext.Set<T>().FindAsync(id);

            if (item is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            return item;
        }

        public async Task<T> AddAsync(T item)
        {
            await dbContext.Set<T>().AddAsync(item);

            return item;
        }
        
        public virtual async Task<T> UpdateAsync(int id, T item)
        {
            var sourceItem = await dbContext.Set<T>().FindAsync(id);

            if (sourceItem is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            dbContext.Entry(sourceItem).CurrentValues.SetValues(item);

            return sourceItem;
        }

        public async Task DeleteAsync(int id)
        {
            var sourceItem = await dbContext.Set<T>().FindAsync(id);

            if (sourceItem is null)
            {
                return;
            }

            dbContext.Remove(sourceItem);
        }

        public async Task<int> CountAsync()
        {
            return await dbContext.Set<T>().CountAsync();
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }
    }
}
