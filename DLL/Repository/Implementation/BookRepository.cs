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
                .AsNoTracking()
                .FirstOrDefaultAsync(book => book.Id == id);
        }
    }
}