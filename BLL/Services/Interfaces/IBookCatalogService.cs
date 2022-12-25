using BLL.DTO.Book;
using DLL.Models;

namespace BLL.Services.Interfaces
{
    public interface IBookCatalogService
    {
        IEnumerable<Book> GetAll();
        Task<Book> FindAsync(int id);
        Task<Book> AddAsync(CreateBookDto item);
        Task<Book> Update(UpdateBookDto item);
        Task DeleteAsync(int id);
        int Count();
    }
}
