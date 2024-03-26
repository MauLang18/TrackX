﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackX.Domain.Entities;

namespace TrackX.Infrastructure.Persistences.Contexts.Configurations
{
    public class TbItinerarioConfiguration : IEntityTypeConfiguration<TbItinerario>
    {
        public void Configure(EntityTypeBuilder<TbItinerario> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__TB_ITINERARIOS__3214EC07ADB85605");

            builder.ToTable("TB_ITINERARIOS");

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
            builder.Property(e => e.Origen)
                .IsUnicode(false);
            builder.Property(e => e.Destino)
                .IsUnicode(false);
            builder.Property(e => e.Transporte)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Modalidad)
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}