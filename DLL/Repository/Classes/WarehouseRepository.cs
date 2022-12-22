using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(ShopDbContext context) : base(context)
        {
        }
    }
}
