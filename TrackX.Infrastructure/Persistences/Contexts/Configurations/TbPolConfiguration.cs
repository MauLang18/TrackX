using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations;

public class TbPolConfiguration : IEntityTypeConfiguration<TbPol>
{
    public void Configure(EntityTypeBuilder<TbPol> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TB_POL__3214EC07A68BB933");
        builder.ToTable("TB_POL");

        builder.Property(e => e.Nombre)
            .HasMaxLength(100)
            .IsUnicode(false);
    }
}