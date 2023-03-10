using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class PaymentWayConfiguration : IEntityTypeConfiguration<PaymentWay>
    {
        public void Configure(EntityTypeBuilder<PaymentWay> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            entityBuilder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
