using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            entityBuilder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.Address)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.City)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.Country)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.PhoneNumber)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
