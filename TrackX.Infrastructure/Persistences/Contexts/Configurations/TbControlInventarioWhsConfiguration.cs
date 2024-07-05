using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations;

public class TbControlInventarioWhsConfiguration : IEntityTypeConfiguration<TbControlInventarioWhs>
{
    public void Configure(EntityTypeBuilder<TbControlInventarioWhs> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TB_CONTROLINVENTARIO__3214EC0711827C4E");
        builder.ToTable("TB_CONTROLINVENTARIO");

        builder.Property(e => e.Cliente).IsUnicode(false);
        builder.Property(e => e.NombreCliente).IsUnicode(false);
        builder.Property(e => e.ControlInventario).IsUnicode(false);
        builder.Property(e => e.Pol).IsUnicode(false);
    }
}