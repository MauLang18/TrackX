using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbUsuarioConfiguration : IEntityTypeConfiguration<TbUsuario>
    {
        public void Configure(EntityTypeBuilder<TbUsuario> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_USUAR__3214EC07B6AFCCFB");
            builder.ToTable("TB_USUARIO");

            builder.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Cliente).IsUnicode(false);
            builder.Property(e => e.Correo).IsUnicode(false);
            builder.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Pass).IsUnicode(false);
            builder.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}