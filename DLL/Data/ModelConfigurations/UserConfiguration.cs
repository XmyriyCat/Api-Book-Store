using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            entityBuilder.Property(x => x.Username)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.Login)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.Password)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.Country)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.City)
                .HasMaxLength(150)
                .IsRequired();

            entityBuilder.Property(x => x.Address)
                .HasMaxLength(150)
                .IsRequired();

            // many to many
            entityBuilder.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity("UserRoles");
        }
    }
}
