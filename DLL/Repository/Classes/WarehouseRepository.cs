using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Classes
{
    public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(DbContext context) : base(context)
        {
        }
    }
}
