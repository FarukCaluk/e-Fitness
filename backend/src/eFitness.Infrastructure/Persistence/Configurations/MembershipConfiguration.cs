using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasMany(m => m.Payments)
            .WithOne(p => p.Membership)
            .HasForeignKey(p => p.MembershipId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(m => new { m.MemberId, m.Status });
    }
}
