using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Data.ModelConfigurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                .IsRequired();

            entityBuilder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            // many to many
            entityBuilder.HasMany(x => x.Books)
                .WithMany(x => x.Genres)
                .UsingEntity(x => x.ToTable("BookGenres"));
        }
    }
}
