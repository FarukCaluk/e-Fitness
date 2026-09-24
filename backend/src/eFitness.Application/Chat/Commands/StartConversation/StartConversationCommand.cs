using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Chat.Commands.StartConversation;

public record StartConversationCommand(int OtherUserId) : IRequest<int>;

public class StartConversationCommandHandler : IRequestHandler<StartConversationCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public StartConversationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(StartConversationCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var currentUserId = _currentUserService.UserId.Value;

        if (currentUserId == request.OtherUserId)
        {
            throw new ConflictException("Cannot start a conversation with yourself.");
        }

        var otherUserExists = await _context.Users.AnyAsync(u => u.Id == request.OtherUserId, cancellationToken);
        if (!otherUserExists)
        {
            throw new NotFoundException(nameof(User), request.OtherUserId);
        }

        var existing = await _context.Conversations.FirstOrDefaultAsync(c =>
            (c.ParticipantOneId == currentUserId && c.ParticipantTwoId == request.OtherUserId) ||
            (c.ParticipantOneId == request.OtherUserId && c.ParticipantTwoId == currentUserId),
            cancellationToken);

        if (existing is not null)
        {
            return existing.Id;
        }

        var conversation = new Conversation
        {
            ParticipantOneId = currentUserId,
            ParticipantTwoId = request.OtherUserId
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(cancellationToken);

        return conversation.Id;
    }
}
