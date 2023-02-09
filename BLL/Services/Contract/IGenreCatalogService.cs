using BLL.DTO.Genre;
using DLL.Models;

namespace BLL.Services.Contract
{
    public interface IGenreCatalogService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> FindAsync(int id);
        Task<Genre> AddAsync(CreateGenreDto item);
        Task<Genre> UpdateAsync(UpdateGenreDto item);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}