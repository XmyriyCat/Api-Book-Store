using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShopDbContext context) : base(context)
        {
        }
    }
}