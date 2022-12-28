using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetAllIncludeAsync();
        Task<Book> FindIncludeAsync(int id);
    }
}
