using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbItinerarioConfiguration : IEntityTypeConfiguration<TbItinerario>
    {
        public void Configure(EntityTypeBuilder<TbItinerario> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_ITINERARIO__3214EC07ADB85605");

            builder.ToTable("TB_ITINERARIO");

            builder.Property(e => e.POL)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.POD)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Carrier)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Vessel)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Voyage)
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}