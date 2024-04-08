using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbExoneracionConfiguration : IEntityTypeConfiguration<TbExoneracion>
    {
        public void Configure(EntityTypeBuilder<TbExoneracion> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_EXONERACION__3214EC0711827C4E");
            builder.ToTable("TB_EXONERACION");

            builder.Property(e => e.Cliente).IsUnicode(false);
            builder.Property(e => e.Descripcion).IsUnicode(false);
            builder.Property(e => e.Idtra)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Solicitud).IsUnicode(false);
            builder.Property(e => e.Autorizacion).IsUnicode(false);

            builder.Property(e => e.ClasificacionArancelaria)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.StatusExoneracion)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Producto)
                .HasMaxLength(500)
                .IsUnicode(false);
            builder.Property(e => e.TipoExoneracion)
                .HasMaxLength(10)
                .IsUnicode(false);
            builder.Property(e => e.NumeroAutorizacion)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.NumeroSolicitud)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.NombreCliente).IsUnicode(false);
        }
    }
}