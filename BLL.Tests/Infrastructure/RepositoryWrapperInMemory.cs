using DLL.Data;
using DLL.Repository.UnitOfWork;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BLL.Tests.Infrastructure
{
    public class RepositoryWrapperInMemory : RepositoryWrapper
    {
        public RepositoryWrapperInMemory(ShopDbContext dbDbContext) : base(dbDbContext)
        {
        }
    }
}
