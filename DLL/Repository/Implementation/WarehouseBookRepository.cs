using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class WarehouseBookRepository : GenericRepository<WarehouseBook>, IWarehouseBookRepository
    {
        public WarehouseBookRepository(DbContext context) : base(context)
        {
        }
    }
}