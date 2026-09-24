using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(m => m.Id);

        builder.HasIndex(m => m.UserId).IsUnique();

        builder.Property(m => m.Gender)
            .HasMaxLength(20);

        builder.Property(m => m.Address)
            .HasMaxLength(250);

        builder.Property(m => m.City)
            .HasMaxLength(100);

        builder.Property(m => m.EmergencyContactName)
            .HasMaxLength(150);

        builder.Property(m => m.EmergencyContactPhone)
            .HasMaxLength(30);

        builder.HasMany(m => m.Memberships)
            .WithOne(ms => ms.Member)
            .HasForeignKey(ms => ms.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.TrainingSessions)
            .WithOne(s => s.Member)
            .HasForeignKey(s => s.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.WorkoutPlans)
            .WithOne(w => w.Member)
            .HasForeignKey(w => w.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Orders)
            .WithOne(o => o.Member)
            .HasForeignKey(o => o.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Payments)
            .WithOne(p => p.Member)
            .HasForeignKey(p => p.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.ProgressLogs)
            .WithOne(pl => pl.Member)
            .HasForeignKey(pl => pl.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
