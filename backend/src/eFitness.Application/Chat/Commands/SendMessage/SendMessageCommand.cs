using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Chat.Commands.SendMessage;

public record SendMessageCommand(int ConversationId, string Content) : IRequest<int>;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SendMessageCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var conversation = await _context.Conversations.FirstOrDefaultAsync(c => c.Id == request.ConversationId, cancellationToken);

        if (conversation is null)
        {
            throw new NotFoundException(nameof(Conversation), request.ConversationId);
        }

        if (conversation.ParticipantOneId != _currentUserService.UserId && conversation.ParticipantTwoId != _currentUserService.UserId)
        {
            throw new ForbiddenAccessException();
        }

        var message = new ChatMessage
        {
            ConversationId = conversation.Id,
            SenderId = _currentUserService.UserId.Value,
            Content = request.Content,
            SentAt = DateTime.UtcNow
        };

        conversation.LastMessageAt = message.SentAt;

        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}
