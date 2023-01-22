using BLL.DTO.OrderLine;
using DLL.Models;

namespace BLL.Services.Contract;

public interface IOrderLineCatalogService
{
    Task<IEnumerable<OrderLine>> GetAllAsync();
    Task<OrderLine> FindAsync(int id);
    Task<OrderLine> AddAsync(CreateOrderLineDto item);
    Task<OrderLine> UpdateAsync(UpdateOrderLineDto item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}