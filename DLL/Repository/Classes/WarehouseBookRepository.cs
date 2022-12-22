using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Classes
{
    public class WarehouseBookRepository : GenericRepository<WarehouseBook>, IWarehouseBookRepository
    {
        public WarehouseBookRepository(DbContext context) : base(context)
        {
        }
    }
}