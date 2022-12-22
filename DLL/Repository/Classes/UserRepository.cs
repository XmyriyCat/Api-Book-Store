using DLL.Data;
using DLL.Models;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ShopDbContext context) : base(context)
        {
        }
    }
}