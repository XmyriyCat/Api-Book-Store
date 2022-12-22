using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class WarehouseBookRepository : GenericRepository<WarehouseBook>, IWarehouseBookRepository
    {
        public WarehouseBookRepository(ShopDbContext context) : base(context)
        {
        }
    }
}