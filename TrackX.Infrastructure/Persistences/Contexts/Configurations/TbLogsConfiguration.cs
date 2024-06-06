using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbLogsConfiguration : IEntityTypeConfiguration<TbLogs>
    {
        public void Configure(EntityTypeBuilder<TbLogs> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_LOGS__3214EC07ADB85605");

            builder.ToTable("TB_LOGS");

            builder.Property(e => e.Modulo)
                .IsUnicode(false);
            builder.Property(e => e.Usuario)
                .IsUnicode(false);
            builder.Property(e => e.TipoMetodo)
                .IsUnicode(false);
            builder.Property(e => e.Parametros)
                .IsUnicode(false);
        }
    }
}