using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations;

public class TbBcfConfiguration : IEntityTypeConfiguration<TbBcf>
{
    public void Configure(EntityTypeBuilder<TbBcf> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TB_DESTINO__3214EC07A68BB933");
        builder.ToTable("TB_BCF");

        builder.Property(e => e.IDTRA)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.BCF)
            .IsUnicode(false);
        builder.Property(e => e.Cliente)
            .IsUnicode(false);
    }
}