using DLL.Data;
using DLL.Models;
using DLL.Repository.Contract;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository.Implementation
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ShopDbContext context) : base(context)
        {
        }
    }
}