using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;

namespace MusicStore.DataAccess.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    // Este tipo de configuracion se llama FluentAPI

    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.Property(p => p.Name)
        // .HasColumnName("T0001STR_NM")
            .HasMaxLength(100);

        // builder.ToTable("T0001");
    }
}