using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbEmpleoConfiguration : IEntityTypeConfiguration<TbEmpleo>
    {
        public void Configure(EntityTypeBuilder<TbEmpleo> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_Empleo__3214EC07ADB85605");

            builder.ToTable("TB_EMPLEO");

            builder.Property(e => e.Titulo)
                .IsUnicode(false);
            builder.Property(e => e.Puesto).IsUnicode(false);
            builder.Property(e => e.Descripcion)
                .IsUnicode(false);
            builder.Property(e => e.Imagen).IsUnicode(false);
        }
    }
}