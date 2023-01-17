using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class WarehouseBookRepository : GenericRepository<WarehouseBook>, IWarehouseBookRepository
    {
        public WarehouseBookRepository(ShopDbContext context) : base(context)
        {
        }
    }
}