using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations;

public class TbNoticiaConfiguration : IEntityTypeConfiguration<TbNoticia>
{
    public void Configure(EntityTypeBuilder<TbNoticia> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TB_NOTICIA__3214EC07ADB85605");

        builder.ToTable("TB_NOTICIA");

        builder.Property(e => e.Titulo)
            .IsUnicode(false);
        builder.Property(e => e.Subtitulo).IsUnicode(false);
        builder.Property(e => e.Contenido)
            .IsUnicode(false);
        builder.Property(e => e.Imagen).IsUnicode(false);
    }
}