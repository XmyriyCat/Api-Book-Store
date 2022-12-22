using System.Linq.Expressions;

namespace DLL.Repository;

public interface IRepository<T> : IDisposable where T : class
{
    IQueryable<T> GetAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    T FindById(int id);
    T Add(T item);
    T Update(T item);
    void Delete(int id);
    int Count();
    void SaveChanges();
}