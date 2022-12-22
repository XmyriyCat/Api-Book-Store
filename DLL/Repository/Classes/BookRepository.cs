using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(ShopDbContext context) : base(context)
        {
        }
    }
}