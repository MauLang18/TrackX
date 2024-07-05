using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations;

public class TbPodConfiguration : IEntityTypeConfiguration<TbPod>
{
    public void Configure(EntityTypeBuilder<TbPod> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TB_POD__3214EC07A68BB933");
        builder.ToTable("TB_POD");

        builder.Property(e => e.Nombre)
            .HasMaxLength(100)
            .IsUnicode(false);
    }
}