using DLL.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builderDb = new DbContextOptionsBuilder<ShopDbContext>().UseInMemoryDatabase("bookShopDbTests")
                .UseInternalServiceProvider(serviceProvider);

            return builderDb.Options;
        }
    }
}
