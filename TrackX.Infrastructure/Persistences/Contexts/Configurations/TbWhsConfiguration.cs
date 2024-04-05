using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbWhsConfiguration : IEntityTypeConfiguration<TbWhs>
    {
        public void Configure(EntityTypeBuilder<TbWhs> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_WHS__3214EC0711827C4E");
            builder.ToTable("TB_WHS");

            builder.Property(e => e.CantidadBultos)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Cliente).IsUnicode(false);
            builder.Property(e => e.Detalle).IsUnicode(false);
            builder.Property(e => e.Documentoregistro).IsUnicode(false);
            builder.Property(e => e.Idtra)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Imagen1).IsUnicode(false);
            builder.Property(e => e.Imagen2).IsUnicode(false);
            builder.Property(e => e.Imagen3).IsUnicode(false);
            builder.Property(e => e.Imagen4).IsUnicode(false);
            builder.Property(e => e.Imagen5).IsUnicode(false);
            builder.Property(e => e.PO)
                .IsUnicode(false);
            builder.Property(e => e.POD)
                .HasMaxLength(200)
                .IsUnicode(false);
            builder.Property(e => e.POL)
                .HasMaxLength(200)
                .IsUnicode(false);
            builder.Property(e => e.StatusWHS)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.TipoBultos)
                .HasMaxLength(200)
                .IsUnicode(false);
            builder.Property(e => e.TipoRegistro)
                .HasMaxLength(10)
                .IsUnicode(false);
            builder.Property(e => e.VinculacionOtroRegistro)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.WHSReceipt)
                .IsUnicode(false);
            builder.Property(e => e.NumeroWHS)
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}