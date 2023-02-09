using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class WarehouseBookConfiguration : IEntityTypeConfiguration<WarehouseBook>
    {
        public void Configure(EntityTypeBuilder<WarehouseBook> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            entityBuilder.Property(x => x.Quantity)
                .IsRequired();

            // one to many
            entityBuilder.HasOne(x => x.Warehouse)
                .WithMany(x => x.WarehouseBooks)
                .HasForeignKey(x => x.WarehouseId);

            // one to many
            entityBuilder.HasOne(x => x.Book)
                .WithMany(x => x.WarehouseBooks)
                .HasForeignKey(x => x.BookId);
        }
    }
}
