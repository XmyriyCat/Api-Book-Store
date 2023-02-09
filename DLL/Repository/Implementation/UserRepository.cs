using System.Linq.Expressions;
using DLL.Data;
using DLL.Errors;
using DLL.Models;
using DLL.Repository.Contract;
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
            var result = await dbContext.Set<User>().AnyAsync(x => x.Login == login);
            return !result;
        }

        public override async Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> expression)
        {
            return await dbContext.Set<User>()
                .Include(user => user.Roles)
                .Include(user => user.Orders)
                .FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<User>> GetAllIncludeAsync()
        {
            return await dbContext.Set<User>()
                .Include(user => user.Roles)
                .Include(user => user.Orders)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> FindIncludeAsync(int id)
        {
            var item = await dbContext.Set<User>()
                .Include(user => user.Roles)
                .Include(user => user.Orders)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item is null)
            {
                throw new DbEntityNotFoundException($"{nameof(item)} with id:{id} is not found in database.");
            }

            return item;
        }
    }
}