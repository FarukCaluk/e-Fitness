using eFitness.Domain.Common;
using eFitness.Domain.Enums;

namespace eFitness.Domain.Entities;

public class Announcement : BaseAuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public AnnouncementSegment Segment { get; set; } = AnnouncementSegment.AllMembers;
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
}
