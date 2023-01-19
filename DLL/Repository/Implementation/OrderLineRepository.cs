using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;

namespace DLL.Repository.Implementation
{
    public class OrderLineRepository : GenericRepository<OrderLine>, IOrderLineRepository
    {
        public OrderLineRepository(ShopDbContext context) : base(context)
        {
        }
    }
}
