using DLL.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Web_Api.Tests.Startup.DbSettings;

namespace Web_Api.Tests.Startup
{
    public class WebApplicationFactoryTest<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptorDb = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<ShopDbContext>));

                services.Remove(descriptorDb); // Removing source DB connection

                services.AddDbContext<ShopDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting"); // adding DB in memory for testing
                });
                
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ShopDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<WebApplicationFactoryTest<TProgram>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        DbUtilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
