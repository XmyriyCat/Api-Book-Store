using BLL.DTO.Book;
using DLL.Models;

namespace BLL.Services.Contract
{
    public interface IBookCatalogService
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> FindAsync(int id);
        Task<Book> AddAsync(CreateBookDto item);
        Task<Book> UpdateAsync(UpdateBookDto item);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
