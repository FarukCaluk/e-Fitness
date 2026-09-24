using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
    {
        builder.ToTable("WorkoutPlans");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(w => w.Description)
            .HasMaxLength(1000);

        builder.HasMany(w => w.Exercises)
            .WithOne(e => e.WorkoutPlan)
            .HasForeignKey(e => e.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class WorkoutPlanExerciseConfiguration : IEntityTypeConfiguration<WorkoutPlanExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutPlanExercise> builder)
    {
        builder.ToTable("WorkoutPlanExercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.DayOfWeek)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(e => e.Exercise)
            .WithMany(ex => ex.WorkoutPlanExercises)
            .HasForeignKey(e => e.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
