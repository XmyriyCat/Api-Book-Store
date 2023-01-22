using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IOrderLineRepository : IRepository<OrderLine>
    {
        Task<IEnumerable<OrderLine>> GetAllIncludeAsync();

        Task<OrderLine> FindIncludeAsync(int id);
    }
}
