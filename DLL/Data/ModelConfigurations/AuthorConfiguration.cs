using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            entityBuilder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(150);

            entityBuilder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(150);

            // many to many
            entityBuilder.HasMany(x => x.Books)
                .WithMany(x => x.Authors)
                .UsingEntity(x => x.ToTable("BookAuthors"));
        }
    }
}
