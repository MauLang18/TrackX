using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations;

public class TbFinanceConfiguration : IEntityTypeConfiguration<TbFinance>
{
    public void Configure(EntityTypeBuilder<TbFinance> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TB_FINANCE__3214EC0711827C4E");
        builder.ToTable("TB_FINANCE");

        builder.Property(e => e.Cliente).IsUnicode(false);
        builder.Property(e => e.EstadoCuenta).IsUnicode(false);
        builder.Property(e => e.NombreCliente).IsUnicode(false);
    }
}