using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("Exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.MuscleGroup)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Difficulty)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.VideoUrl)
            .HasMaxLength(500);

        builder.HasOne(e => e.Equipment)
            .WithMany(eq => eq.Exercises)
            .HasForeignKey(e => e.EquipmentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
