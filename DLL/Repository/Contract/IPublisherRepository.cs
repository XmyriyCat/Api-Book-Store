using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        Task<IEnumerable<Publisher>> GetAllIncludeAsync();
        Task<Publisher> FindIncludeAsync(int id);
    }
}
