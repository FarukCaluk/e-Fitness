using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Chat.Queries.GetMessages;

public record GetMessagesQuery(int ConversationId, int PageNumber, int PageSize) : IRequest<PaginatedList<ChatMessageDto>>;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, PaginatedList<ChatMessageDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMessagesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<ChatMessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var conversation = await _context.Conversations.FirstOrDefaultAsync(c => c.Id == request.ConversationId, cancellationToken);

        if (conversation is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Conversation), request.ConversationId);
        }

        if (conversation.ParticipantOneId != _currentUserService.UserId && conversation.ParticipantTwoId != _currentUserService.UserId)
        {
            throw new ForbiddenAccessException();
        }

        var query = _context.ChatMessages
            .Include(m => m.Sender)
            .Where(m => m.ConversationId == request.ConversationId)
            .OrderByDescending(m => m.SentAt);

        var unreadMessages = await _context.ChatMessages
            .Where(m => m.ConversationId == request.ConversationId && m.SenderId != _currentUserService.UserId && !m.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var message in unreadMessages)
        {
            message.IsRead = true;
        }

        if (unreadMessages.Count > 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        var paged = await PaginatedList<Domain.Entities.ChatMessage>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(ChatMessageDto.FromEntity).OrderBy(m => m.SentAt).ToList();

        return new PaginatedList<ChatMessageDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
