using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;

namespace DLL.Repository.Implementation
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShopDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetAllIncludeAsync()
        {
            return await dbContext.Set<Order>()
                .Include(order => order.User).ThenInclude(user => user.Roles)
                .Include(order => order.Shipment).ThenInclude(shipment => shipment.PaymentWay)
                .Include(order => order.Shipment).ThenInclude(shipment => shipment.Delivery)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order> FindIncludeAsync(int id)
        {
            var item = await dbContext.Set<Order>()
                .Include(order => order.User).ThenInclude(user => user.Roles)
                .Include(order => order.Shipment).ThenInclude(shipment => shipment.PaymentWay)
                .Include(order => order.Shipment).ThenInclude(shipment => shipment.Delivery)
                .FirstOrDefaultAsync(book => book.Id == id);

            if (item is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            return item;
        }

        public override async Task<Order> UpdateAsync(int id, Order item)
        {
            var sourceItem = await FindIncludeAsync(id);

            if (sourceItem is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            dbContext.Entry(sourceItem).CurrentValues.SetValues(item);

            return sourceItem;
        }
    }
}