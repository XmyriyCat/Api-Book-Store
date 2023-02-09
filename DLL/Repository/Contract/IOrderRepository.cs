using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllIncludeAsync();
        
        Task<Order> FindIncludeAsync(int id);
    }
}