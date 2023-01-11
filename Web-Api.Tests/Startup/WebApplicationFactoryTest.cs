using DLL.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Web_Api.Tests.Errors;

namespace Web_Api.Tests.Startup
{
    public class WebApplicationFactoryTest<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ShopDbContext>));

                if (descriptor is null)
                {
                    throw new DbContextIsNotFoundException("DB context is not found in source web application");
                }

                services.Remove(descriptor); // removing source DB connection

                services.AddDbContext<ShopDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting"); // adding db in memory for testing
                });

                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ShopDbContext>();
                    //db.Database.EnsureCreated();
                }
            });

        }
    }
}
