using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
{
    public void Configure(EntityTypeBuilder<MembershipPlan> builder)
    {
        builder.ToTable("MembershipPlans");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)");

        builder.HasMany(p => p.Features)
            .WithOne(f => f.MembershipPlan)
            .HasForeignKey(f => f.MembershipPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Memberships)
            .WithOne(m => m.MembershipPlan)
            .HasForeignKey(m => m.MembershipPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class MembershipPlanFeatureConfiguration : IEntityTypeConfiguration<MembershipPlanFeature>
{
    public void Configure(EntityTypeBuilder<MembershipPlanFeature> builder)
    {
        builder.ToTable("MembershipPlanFeatures");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Description)
            .IsRequired()
            .HasMaxLength(250);
    }
}
