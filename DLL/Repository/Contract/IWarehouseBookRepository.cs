using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IWarehouseBookRepository : IRepository<WarehouseBook>
    {
        Task<IEnumerable<WarehouseBook>> GetAllIncludeAsync();
        Task<WarehouseBook> FindIncludeAsync(int id);
    }
}
