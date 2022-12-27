using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context) : base(context)
        {
        }
    }
}