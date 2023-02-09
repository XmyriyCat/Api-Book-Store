using System.Linq.Expressions;

namespace DLL.Repository;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    Task<T> FindAsync(int id);
    Task<T> AddAsync(T item);
    Task<T> UpdateAsync(int id, T item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
}