using System.Linq.Expressions;
using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ShopDbContext context) : base(context)
        {
        }

        public async Task<bool> IsUniqueLoginAsync(string login)
        {
            bool result = await dbContext.Set<User>().AnyAsync(x => x.Login == login);
            return !result;
        }

        public override async Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> expression)
        {
            return await dbContext.Set<User>()
                .Include(user => user.Roles)
                .Include(user => user.Orders)
                .FirstOrDefaultAsync(expression);
        }
    }
}