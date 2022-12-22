using DLL.Models;
using DLL.Data;
using DLL.Repository.Interfaces;

namespace DLL.Repository.Classes
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ShopDbContext context) : base(context)
        {
        }
    }
}