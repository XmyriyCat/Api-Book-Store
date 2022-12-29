using BLL.DTO.Publisher;
using DLL.Models;

namespace BLL.Services.Contract
{
    public interface IPublisherCatalogService
    {
        Task<IEnumerable<Publisher>> GetAllAsync();
        Task<Publisher> FindAsync(int id);
        Task<Publisher> AddAsync(CreatePublisherDto item);
        Task<Publisher> UpdateAsync(UpdatePublisherDto item);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}