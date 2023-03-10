using DLL.Models;

namespace DLL.Repository.Contract
{
    public interface IShipmentRepository : IRepository<Shipment>
    {
        Task<IEnumerable<Shipment>> GetAllIncludeAsync();

        Task<Shipment> FindIncludeAsync(int id);
    }
}
