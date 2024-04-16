using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbPrivilegioConfiguration : IEntityTypeConfiguration<TbPrivilegio>
    {
        public void Configure(EntityTypeBuilder<TbPrivilegio> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_PRIVILEGIO__3214EC07A68BB933");
            builder.ToTable("TB_PRIVILEGIO");

            builder.Property(e => e.Nombre)
                .HasMaxLength(25)
                .IsUnicode(false);
            builder.Property(e => e.Descripcion)
                .IsUnicode(false);
        }
    }
}