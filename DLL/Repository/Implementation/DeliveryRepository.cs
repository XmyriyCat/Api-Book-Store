using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;

namespace DLL.Repository.Implementation
{
    public class DeliveryRepository : GenericRepository<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(ShopDbContext context) : base(context)
        {
        }
    }
}