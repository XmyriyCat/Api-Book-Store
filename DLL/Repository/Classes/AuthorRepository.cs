using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ShopDbContext context) : base(context)
        {
        }
    }
}