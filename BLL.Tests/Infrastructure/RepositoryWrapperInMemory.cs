using DLL.Data;
using DLL.Repository.UnitOfWork;

namespace BLL.Tests.Infrastructure
{
    public class RepositoryWrapperInMemory : RepositoryWrapper
    {
        public RepositoryWrapperInMemory(ShopDbContext dbDbContext) : base(dbDbContext)
        {
        }
    }
}
