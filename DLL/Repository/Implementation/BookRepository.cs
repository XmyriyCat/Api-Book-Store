using DLL.Errors;
using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetAllIncludeAsync()
        {
            return await dbContext.Set<Book>()
            .Include(book => book.Authors)
            .Include(book => book.Genres)
            .Include(book => book.Publisher)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<Book> FindIncludeAsync(int id)
        {
            return await dbContext.Set<Book>()
                .Include(book => book.Authors)
                .Include(book => book.Genres)
                .Include(book => book.Publisher)
                .FirstOrDefaultAsync(book => book.Id == id);
        }

        public override async Task<Book> UpdateAsync(int id, Book item)
        {
            var sourceItem = await FindIncludeAsync(id);

            if (sourceItem is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            dbContext.Entry(sourceItem).CurrentValues.SetValues(item);

            // updating collections in item properties
            sourceItem.Authors = item.Authors;
            sourceItem.Genres = item.Genres;

            return sourceItem;
        }
    }
}