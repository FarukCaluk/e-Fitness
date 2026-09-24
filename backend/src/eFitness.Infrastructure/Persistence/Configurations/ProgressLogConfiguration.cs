using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class ProgressLogConfiguration : IEntityTypeConfiguration<ProgressLog>
{
    public void Configure(EntityTypeBuilder<ProgressLog> builder)
    {
        builder.ToTable("ProgressLogs");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.WeightKg)
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.BodyFatPercentage)
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.MuscleMassKg)
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.Notes)
            .HasMaxLength(500);

        builder.HasIndex(p => new { p.MemberId, p.RecordedAt });
    }
}
