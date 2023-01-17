using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class PaymentWayRepository : GenericRepository<PaymentWay>, IPaymentWayRepository
    {
        public PaymentWayRepository(ShopDbContext context) : base(context)
        {
        }
    }
}