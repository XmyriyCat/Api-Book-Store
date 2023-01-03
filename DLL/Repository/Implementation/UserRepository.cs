using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<bool> IsUniqueLoginAsync(string login)
        {
            bool result = await dbContext.Set<User>().AnyAsync(x => x.Login == login);
            return !result;
        }
    }
}