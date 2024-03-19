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
            builder.HasKey(e => e.Id).HasName("PK__TB_USUAR__3214EC07ADB85605");

            builder.ToTable("TB_USUARIO");

            builder.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Cliente).IsUnicode(false);
            builder.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.Pass)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Tipo)
                .HasMaxLength(7)
                .IsUnicode(false);
            builder.Property(e => e.Imagen).IsUnicode(false);

            builder.HasOne(d => d.IdRolNavigation).WithMany(p => p.TbUsuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROL_USUARIO");
        }
    }
}