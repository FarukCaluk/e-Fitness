using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFitness.Infrastructure.Persistence.Configurations;

public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("Announcements");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Body)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(a => a.Segment)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}
