using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbDestinoConfiguration : IEntityTypeConfiguration<TbDestino>
    {
        public void Configure(EntityTypeBuilder<TbDestino> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_DESTINO__3214EC07A68BB933");
            builder.ToTable("TB_DESTINO");

            builder.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Imagen)
                .IsUnicode(false);
        }
    }
}