using System.Linq.Expressions;
using DLL.Data;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        private bool _isDisposed;
        private readonly ShopDbContext _dbContext;

        protected GenericRepository(ShopDbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression);
        }
        public T FindById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T Add(T item)
        {
            _dbContext.Set<T>().Add(item);
            return item;
        }
        public T Update(T item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            return item;
        }
        public void Delete(int id)
        {
            var sourceItem = _dbContext.Set<T>().Find(id);
            if (sourceItem is null)
            {
                return;
            }

            _dbContext.Remove(sourceItem);
        }

        public int Count()
        {
            return _dbContext.Set<T>().Count();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
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
