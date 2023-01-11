using DLL.Data;
using Microsoft.EntityFrameworkCore;

namespace BLL.Tests.Infrastructure
{
    public static class DbInMemory
    {
        public static ShopDbContext CreateDbContextInMemory()
        {
            var dbContextOptions = CreateDbContextOptionsInMemory();
            var dbContextInMemory = new ShopDbContext(dbContextOptions);
            return dbContextInMemory;
        }

        private static DbContextOptions<ShopDbContext> CreateDbContextOptionsInMemory()
        {
            return new DbContextOptionsBuilder<ShopDbContext>().UseInMemoryDatabase("bookShopDbTests").Options;
        }
    }
}
