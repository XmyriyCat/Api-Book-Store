using BLL.DTO.Author;
using DLL.Models;

namespace BLL.Services.Interfaces
{
    public interface IAuthorCatalogService
    {
        IEnumerable<Author> GetAll();
        Task<Author> FindAsync(int id);
        Task<Author> AddAsync(CreateAuthorDto item);
        Task<Author> Update(UpdateAuthorDto item);
        Task DeleteAsync(int id);
        int Count();
    }
}
