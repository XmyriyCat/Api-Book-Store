using BLL.DTO.Warehouse;
using DLL.Models;

namespace BLL.Services.Contract;

public interface IWarehouseCatalogService
{
    Task<IEnumerable<Warehouse>> GetAllAsync();
    Task<Warehouse> FindAsync(int id);
    Task<Warehouse> AddAsync(CreateWarehouseDto item);
    Task<Warehouse> UpdateAsync(UpdateWarehouseDto item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}