using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            // one to many
            entityBuilder.HasOne(x => x.Delivery)
                .WithMany(x => x.Shipment)
                .HasForeignKey(x => x.DeliveryId)
                .IsRequired();

            // one to many
            entityBuilder.HasOne(x => x.PaymentWay)
                .WithMany(x => x.Shipment)
                .HasForeignKey(x => x.PaymentWayId)
                .IsRequired();
        }
    }
}
