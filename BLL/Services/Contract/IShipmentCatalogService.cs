using BLL.DTO.Shipment;
using DLL.Models;

namespace BLL.Services.Contract;

public interface IShipmentCatalogService
{
    Task<IEnumerable<Shipment>> GetAllAsync();
    Task<Shipment> FindAsync(int id);
    Task<Shipment> AddAsync(CreateShipmentDto item);
    Task<Shipment> UpdateAsync(UpdateShipmentDto item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}
