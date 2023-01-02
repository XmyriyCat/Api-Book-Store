using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            entityBuilder.Property(x => x.Quantity)
                .IsRequired();

            // one to many
            entityBuilder.HasOne(x => x.Order)
                .WithMany(x => x.OrderLine)
                .HasForeignKey(x => x.OrderId)
                .IsRequired();

            // one to many
            entityBuilder.HasOne(x => x.WarehouseBook)
                .WithMany(x => x.OrderLines)
                .HasForeignKey(x => x.WarehouseBookId)
                .IsRequired();
        }
    }
}
