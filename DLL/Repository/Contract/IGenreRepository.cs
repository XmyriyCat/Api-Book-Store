using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetAllIncludeAsync();
        Task<Genre> FindIncludeAsync(int id);
    }
}