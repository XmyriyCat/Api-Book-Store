using BLL.DTO.Order;
using DLL.Models;

namespace BLL.Services.Contract;

public interface IOrderCatalogService
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order> FindAsync(int id);
    Task<Order> AddAsync(CreateOrderDto item);
    Task<Order> UpdateAsync(UpdateOrderDto item);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}