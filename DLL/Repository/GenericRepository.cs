using DLL.Errors;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        private bool _isDisposed;
        private readonly DbContext _dbContext;

        protected GenericRepository(DbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }
        
        public async Task<T> FindAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);

            return item;
        }
        
        public async Task<T> UpdateAsync(T item)
        {
            var sourceItem = await _dbContext.Set<T>().FindAsync(item);

            if (sourceItem is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} is not found in database");
            }

            _dbContext.Entry(sourceItem).CurrentValues.SetValues(item);

            return sourceItem;
        }

        public async Task DeleteAsync(int id)
        {
            var sourceItem = await _dbContext.Set<T>().FindAsync(id);

            if (sourceItem is null)
            {
                return;
            }

            _dbContext.Remove(sourceItem);
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                _isDisposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
