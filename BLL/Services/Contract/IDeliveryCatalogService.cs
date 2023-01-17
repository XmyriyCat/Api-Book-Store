using BLL.DTO.Delivery;
using DLL.Models;

namespace BLL.Services.Contract
{
    public interface IDeliveryCatalogService
    {
        Task<IEnumerable<Delivery>> GetAllAsync();
        Task<Delivery> FindAsync(int id);
        Task<Delivery> AddAsync(CreateDeliveryDto item);
        Task<Delivery> UpdateAsync(UpdateDeliveryDto item);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
