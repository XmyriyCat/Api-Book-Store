using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class PaymentWayRepository : GenericRepository<PaymentWay>, IPaymentWayRepository
    {
        public PaymentWayRepository(ShopDbContext context) : base(context)
        {
        }
    }
}