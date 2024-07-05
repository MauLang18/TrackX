using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations;

public class TbMultimediaConfiguration : IEntityTypeConfiguration<TbMultimedia>
{
    public void Configure(EntityTypeBuilder<TbMultimedia> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TB_MULTIMEDIA__3214EC0711827C4E");
        builder.ToTable("TB_MULTIMEDIA");

        builder.Property(e => e.Nombre)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.Multimedia)
            .IsUnicode(false);
    }
}