using BLL.DTO.WarehouseBook;
using DLL.Models;

namespace BLL.Services.Contract;

public interface IWarehouseBookCatalogService
{
    Task<IEnumerable<WarehouseBook>> GetAllAsync();
    Task<WarehouseBook> FindAsync(int id);
    Task<WarehouseBook> AddAsync(CreateWarehouseBookDto item);
    Task<WarehouseBook> UpdateAsync(UpdateWarehouseBookDto item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}