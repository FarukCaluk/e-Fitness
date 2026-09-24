using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Announcements;

public record AnnouncementDto(
    int Id,
    string Title,
    string Body,
    AnnouncementSegment Segment,
    string AuthorName,
    DateTime CreatedAt)
{
    public static AnnouncementDto FromEntity(Announcement announcement) => new(
        announcement.Id,
        announcement.Title,
        announcement.Body,
        announcement.Segment,
        $"{announcement.Author.FirstName} {announcement.Author.LastName}",
        announcement.CreatedAt);
}
