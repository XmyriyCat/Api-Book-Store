using DLL.Errors;
using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class WarehouseBookRepository : GenericRepository<WarehouseBook>, IWarehouseBookRepository
    {
        public WarehouseBookRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WarehouseBook>> GetAllIncludeAsync()
        {
            return await dbContext.Set<WarehouseBook>()
                .Include(warehouseBook => warehouseBook.Book).ThenInclude(book => book.Publisher)
                .Include(warehouseBook => warehouseBook.Book).ThenInclude(book => book.Genres)
                .Include(warehouseBook => warehouseBook.Book).ThenInclude(book => book.Authors)
                .Include(warehouseBook => warehouseBook.Warehouse)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<WarehouseBook> FindIncludeAsync(int id)
        {
            var item = await dbContext.Set<WarehouseBook>()
                .Include(warehouseBook => warehouseBook.Book).ThenInclude(book => book.Publisher)
                .Include(warehouseBook => warehouseBook.Book).ThenInclude(book => book.Genres)
                .Include(warehouseBook => warehouseBook.Book).ThenInclude(book => book.Authors)
                .Include(warehouseBook => warehouseBook.Warehouse)
                .FirstOrDefaultAsync(book => book.Id == id);

            if (item is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            return item;
        }

        public override async Task<WarehouseBook> UpdateAsync(int id, WarehouseBook item)
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