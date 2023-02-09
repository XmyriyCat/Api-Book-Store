using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;

namespace DLL.Repository.Implementation
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ShopDbContext context) : base(context)
        {
        }
    }
}