using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class OrderLineRepository : GenericRepository<OrderLine>, IOrderLineRepository
    {
        public OrderLineRepository(ShopDbContext context) : base(context)
        {
        }
    }
}
