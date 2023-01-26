using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        Task<IEnumerable<Warehouse>> GetAllIncludeAsync();
        Task<Warehouse> FindIncludeAsync(int id);
    }
}
