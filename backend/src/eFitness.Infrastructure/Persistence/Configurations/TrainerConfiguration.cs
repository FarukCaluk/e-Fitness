using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("Trainers");

        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.UserId).IsUnique();

        builder.Property(t => t.Specialization)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Bio)
            .HasMaxLength(2000);

        builder.Property(t => t.HourlyRate)
            .HasColumnType("decimal(10,2)");

        builder.HasMany(t => t.TrainingSessions)
            .WithOne(s => s.Trainer)
            .HasForeignKey(s => s.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.WorkoutPlans)
            .WithOne(w => w.Trainer)
            .HasForeignKey(w => w.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Clients)
            .WithOne(m => m.AssignedTrainer)
            .HasForeignKey(m => m.AssignedTrainerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
