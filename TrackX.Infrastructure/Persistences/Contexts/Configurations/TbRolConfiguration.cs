using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbRolConfiguration : IEntityTypeConfiguration<TbRol>
    {
        public void Configure(EntityTypeBuilder<TbRol> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_ROL__3214EC07A68BB933");
            builder.ToTable("TB_ROL");

            builder.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}