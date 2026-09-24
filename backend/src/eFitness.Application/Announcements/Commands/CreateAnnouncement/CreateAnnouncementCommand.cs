using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;

namespace eFitness.Application.Announcements.Commands.CreateAnnouncement;

public record CreateAnnouncementCommand(string Title, string Body, AnnouncementSegment Segment) : IRequest<int>;

public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateAnnouncementCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var announcement = new Announcement
        {
            Title = request.Title,
            Body = request.Body,
            Segment = request.Segment,
            AuthorId = _currentUserService.UserId.Value
        };

        _context.Announcements.Add(announcement);
        await _context.SaveChangesAsync(cancellationToken);

        return announcement.Id;
    }
}
