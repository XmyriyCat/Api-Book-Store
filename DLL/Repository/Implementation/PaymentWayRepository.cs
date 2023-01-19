using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;

namespace DLL.Repository.Implementation
{
    public class PaymentWayRepository : GenericRepository<PaymentWay>, IPaymentWayRepository
    {
        public PaymentWayRepository(ShopDbContext context) : base(context)
        {
        }
    }
}