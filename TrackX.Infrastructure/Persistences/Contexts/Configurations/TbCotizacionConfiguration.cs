using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations;

public class TbCotizacionConfiguration : IEntityTypeConfiguration<TbCotizacion>
{
    public void Configure(EntityTypeBuilder<TbCotizacion> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TB_COTIZACION_3214EC07A68BB933");
        builder.ToTable("TB_COTIZACION");

        builder.Property(e => e.QUO)
            .HasMaxLength(100)
            .IsUnicode(false);
        builder.Property(e => e.Cotizacion)
            .IsUnicode(false);
        builder.Property(e => e.Cliente)
            .IsUnicode(false);
    }
}