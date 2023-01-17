using System.Reflection;
using DLL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DLL.Data
{
    public class ShopDbContext : IdentityDbContext<User, Role, int>
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<PaymentWay> PaymentWays { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        //public DbSet<Role> Roles { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseBook> WarehouseBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure all model configurations that implements interface IEntityTypeConfiguration<T> from Assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
