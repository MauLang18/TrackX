using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbRolPrivilegioConfiguration : IEntityTypeConfiguration<TbRolPrivilegio>
    {
        public void Configure(EntityTypeBuilder<TbRolPrivilegio> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_PRIVILEGIO__3214EC07A68BB933");
            builder.ToTable("TB_PRIVILEGIO");

            builder.HasOne(d => d.Rol).WithMany(p => p.TbRolPrivilegio)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_ROL_PRIVILEGIO_TB_ROL__6EF57B66");

            builder.HasOne(d => d.Privilegio).WithMany(p => p.TbRolPrivilegio)
                .HasForeignKey(d => d.IdPrivilegio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TB_ROL_PRIVILEGIO_TB_PRIVILEGIO__6FE99F9F");
        }
    }
}