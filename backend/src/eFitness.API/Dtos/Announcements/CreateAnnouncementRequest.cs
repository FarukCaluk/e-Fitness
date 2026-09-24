using eFitness.Domain.Enums;

namespace eFitness.API.Dtos.Announcements;

public record CreateAnnouncementRequest(string Title, string Body, AnnouncementSegment Segment);
