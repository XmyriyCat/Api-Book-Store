using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class PaymentWayRepository : GenericRepository<PaymentWay>, IPaymentWayRepository
    {
        public PaymentWayRepository(DbContext context) : base(context)
        {
        }
    }
}