using DLL.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiBookStore.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureMsSqlServerContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("ConnectionStringBookDbSql");
            services.AddDbContext<ShopDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
