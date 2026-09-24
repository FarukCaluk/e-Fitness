using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Chat.Queries.GetConversations;

public record GetConversationsQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<ConversationSummaryDto>>;

public class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, PaginatedList<ConversationSummaryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetConversationsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<ConversationSummaryDto>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var currentUserId = _currentUserService.UserId.Value;

        var query = _context.Conversations
            .Include(c => c.ParticipantOne)
            .Include(c => c.ParticipantTwo)
            .Include(c => c.Messages)
            .Where(c => c.ParticipantOneId == currentUserId || c.ParticipantTwoId == currentUserId)
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt);

        var paged = await PaginatedList<Domain.Entities.Conversation>.CreateAsync(query, request.PageNumber, request.PageSize);

        var items = paged.Items.Select(c =>
        {
            var other = c.ParticipantOneId == currentUserId ? c.ParticipantTwo : c.ParticipantOne;
            var lastMessage = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
            var unreadCount = c.Messages.Count(m => !m.IsRead && m.SenderId != currentUserId);

            return new ConversationSummaryDto(
                c.Id,
                other.Id,
                $"{other.FirstName} {other.LastName}",
                lastMessage?.Content,
                lastMessage?.SentAt,
                unreadCount);
        }).ToList();

        return new PaginatedList<ConversationSummaryDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
