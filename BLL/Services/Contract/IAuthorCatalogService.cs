using BLL.DTO.Author;
using DLL.Models;

namespace BLL.Services.Contract
{
    public interface IAuthorCatalogService
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> FindAsync(int id);
        Task<Author> AddAsync(CreateAuthorDto item);
        Task<Author> UpdateAsync(UpdateAuthorDto item);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
