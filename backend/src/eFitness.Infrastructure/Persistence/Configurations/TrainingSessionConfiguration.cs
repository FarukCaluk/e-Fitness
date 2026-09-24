using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class TrainingSessionConfiguration : IEntityTypeConfiguration<TrainingSession>
{
    public void Configure(EntityTypeBuilder<TrainingSession> builder)
    {
        builder.ToTable("TrainingSessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.Notes)
            .HasMaxLength(1000);

        builder.Property(s => s.Location)
            .HasMaxLength(150);

        builder.HasIndex(s => s.ScheduledAt);
        builder.HasIndex(s => new { s.TrainerId, s.ScheduledAt });
    }
}
