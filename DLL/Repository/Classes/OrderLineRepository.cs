using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Classes
{
    public class OrderLineRepository : GenericRepository<OrderLine>, IOrderLineRepository
    {
        public OrderLineRepository(DbContext context) : base(context)
        {
        }
    }
}
