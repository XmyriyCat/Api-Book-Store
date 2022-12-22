using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Classes
{
    public class PaymentWayRepository : GenericRepository<PaymentWay>, IPaymentWayRepository
    {
        public PaymentWayRepository(DbContext context) : base(context)
        {
        }
    }
}