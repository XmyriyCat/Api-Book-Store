using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;

namespace DLL.Repository.Implementation
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ShopDbContext context) : base(context)
        {
        }
    }
}