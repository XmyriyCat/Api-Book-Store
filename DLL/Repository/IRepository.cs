namespace DLL.Repository;

public interface IRepository<T> : IDisposable where T : class
{
    IQueryable<T> GetAll();
    Task<T> FindAsync(int id);
    Task<T> AddAsync(T item);
    T Update(T item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}