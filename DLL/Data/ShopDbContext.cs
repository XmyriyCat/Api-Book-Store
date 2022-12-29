using DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace DLL.Data
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions options) : base(options)
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
        public DbSet<Role> Roles { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseBook> WarehouseBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateAuthorModel(modelBuilder);
            CreateGenreModel(modelBuilder);
            CreatePublisherModel(modelBuilder);
            CreateBookModel(modelBuilder);
            CreateWarehouseBookModel(modelBuilder);
            CreateWarehouseModel(modelBuilder);
            CreateOrderLineModel(modelBuilder);
            CreateOrderModel(modelBuilder);
            CreateUserModel(modelBuilder);
            CreateRoleModel(modelBuilder);
            CreateShipmentModel(modelBuilder);
            CreateDeliveryModel(modelBuilder);
            CreatePaymentWayModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void CreateAuthorModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Author>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Author>()
                .Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Author>()
                .Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(150);

            // many to many
            modelBuilder.Entity<Author>()
                .HasMany(x => x.Books)
                .WithMany(x => x.Authors)
                .UsingEntity(x => x.ToTable("BookAuthors"));
        }

        private void CreateGenreModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Genre>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Genre>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            // many to many
            modelBuilder.Entity<Genre>()
                .HasMany(x => x.Books)
                .WithMany(x => x.Genres)
                .UsingEntity(x => x.ToTable("BookGenres"));
        }

        private void CreatePublisherModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Publisher>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Publisher>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Publisher>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);
        }

        private void CreateBookModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Book>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Book>()
                .Property(x => x.Price)
                .IsRequired();

            // one to many
            modelBuilder.Entity<Book>()
                .HasOne(x => x.Publisher)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.PublisherId)
                .IsRequired();
        }

        private void CreateWarehouseBookModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WarehouseBook>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<WarehouseBook>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<WarehouseBook>()
                .Property(x => x.Quantity)
                .IsRequired();

            // one to many
            modelBuilder.Entity<WarehouseBook>()
                .HasOne(x => x.Warehouse)
                .WithMany(x => x.WarehouseBooks)
                .HasForeignKey(x => x.WarehouseId);

            // one to many
            modelBuilder.Entity<WarehouseBook>()
                .HasOne(x => x.Book)
                .WithMany(x => x.WarehouseBooks)
                .HasForeignKey(x => x.BookId);
        }

        private void CreateWarehouseModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Warehouse>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(x => x.Address)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(x => x.City)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(x => x.Country)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Warehouse>()
                .Property(x => x.PhoneNumber)
                .HasMaxLength(150)
                .IsRequired();
        }

        private void CreateOrderLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLine>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<OrderLine>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<OrderLine>()
                .Property(x => x.Quantity)
                .IsRequired();

            // one to many
            modelBuilder.Entity<OrderLine>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderLine)
                .HasForeignKey(x => x.OrderId)
                .IsRequired();

            // one to many
            modelBuilder.Entity<OrderLine>()
                .HasOne(x => x.WarehouseBook)
                .WithMany(x => x.OrderLines)
                .HasForeignKey(x => x.WarehouseBookId)
                .IsRequired();
        }

        private void CreateOrderModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Order>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(x => x.TotalPrice)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(x => x.OrderDate)
                .IsRequired();

            // one to many
            modelBuilder.Entity<Order>()
                .HasOne(x => x.Shipment)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.ShipmentId)
                .IsRequired();

            // one to many
            modelBuilder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .IsRequired();
        }

        private void CreateUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Login)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Password)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Country)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.City)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Address)
                .HasMaxLength(150)
                .IsRequired();

            // many to many
            modelBuilder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity("UserRoles");
        }

        private void CreateRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Role>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();
        }

        private void CreateShipmentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipment>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Shipment>()
                .Property(x => x.Id)
                .IsRequired();

            // one to many
            modelBuilder.Entity<Shipment>()
                .HasOne(x => x.Delivery)
                .WithMany(x => x.Shipment)
                .HasForeignKey(x => x.DeliveryId)
                .IsRequired();

            // one to many
            modelBuilder.Entity<Shipment>()
                .HasOne(x => x.PaymentWay)
                .WithMany(x => x.Shipment)
                .HasForeignKey(x => x.PaymentWayId)
                .IsRequired();
        }

        private void CreateDeliveryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Delivery>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Delivery>()
                .Property(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<Delivery>()
                .Property(x => x.Price)
                .IsRequired();

            modelBuilder.Entity<Delivery>()
                .Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();
        }

        private void CreatePaymentWayModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentWay>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PaymentWay>()
                .Property(x => x.Id)
                .IsRequired();
            
            modelBuilder.Entity<PaymentWay>()
                .Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
