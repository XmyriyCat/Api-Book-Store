using DLL.Data;
using DLL.Errors;
using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class OrderLineRepository : GenericRepository<OrderLine>, IOrderLineRepository
    {
        public OrderLineRepository(ShopDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderLine>> GetAllIncludeAsync()
        {
            return await dbContext.Set<OrderLine>()
                .Include(orderLine => orderLine.WarehouseBook).ThenInclude(warehouseBook => warehouseBook.Book).ThenInclude(book => book.Publisher)
                .Include(orderLine => orderLine.WarehouseBook).ThenInclude(warehouseBook => warehouseBook.Warehouse)
                .Include(orderLine => orderLine.Order).ThenInclude(order => order.Shipment)
                .Include(orderLine => orderLine.Order).ThenInclude(order => order.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<OrderLine> FindIncludeAsync(int id)
        {
            var item = await dbContext.Set<OrderLine>()
                .Include(orderLine => orderLine.WarehouseBook).ThenInclude(warehouseBook => warehouseBook.Book).ThenInclude(book => book.Publisher)
                .Include(orderLine => orderLine.WarehouseBook).ThenInclude(warehouseBook => warehouseBook.Warehouse)
                .Include(orderLine => orderLine.Order).ThenInclude(order => order.Shipment)
                .Include(orderLine => orderLine.Order).ThenInclude(order => order.User)
                .FirstOrDefaultAsync(orderLine => orderLine.Id == id);

            if (item is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            return item;
        }

        public override async Task<OrderLine> UpdateAsync(int id, OrderLine item)
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
