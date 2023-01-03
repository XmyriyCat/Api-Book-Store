using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            entityBuilder.Property(x => x.TotalPrice)
                .IsRequired();

            entityBuilder.Property(x => x.OrderDate)
                .IsRequired();

            // one to many
            entityBuilder.HasOne(x => x.Shipment)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.ShipmentId)
                .IsRequired();

            // one to many
            entityBuilder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .IsRequired();
        }
    }
}
