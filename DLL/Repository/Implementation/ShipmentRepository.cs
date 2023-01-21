using DLL.Errors;
using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class ShipmentRepository : GenericRepository<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Shipment>> GetAllIncludeAsync()
        {
            return await dbContext.Set<Shipment>()
                .Include(shipment => shipment.Delivery)
                .Include(shipment => shipment.PaymentWay)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Shipment> FindIncludeAsync(int id)
        {
            var item = await dbContext.Set<Shipment>()
                .Include(shipment => shipment.Delivery)
                .Include(shipment => shipment.PaymentWay)
                .FirstOrDefaultAsync(book => book.Id == id);

            if (item is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            return item;
        }

        public override async Task<Shipment> UpdateAsync(int id, Shipment item)
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