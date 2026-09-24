using eFitness.API.Dtos.Chat;
using eFitness.Application.Chat;
using eFitness.Application.Chat.Commands.SendMessage;
using eFitness.Application.Chat.Commands.StartConversation;
using eFitness.Application.Chat.Queries.GetConversations;
using eFitness.Application.Chat.Queries.GetMessages;
using eFitness.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly ISender _sender;

    public ChatController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("conversations")]
    public async Task<ActionResult<PaginatedList<ConversationSummaryDto>>> GetConversations(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetConversationsQuery(pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> StartConversation(StartConversationRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(new StartConversationCommand(request.OtherUserId), cancellationToken);
        return Ok(new { id });
    }

    [HttpGet("conversations/{conversationId:int}/messages")]
    public async Task<ActionResult<PaginatedList<ChatMessageDto>>> GetMessages(
        int conversationId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetMessagesQuery(conversationId, pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpPost("conversations/{conversationId:int}/messages")]
    public async Task<IActionResult> SendMessage(int conversationId, SendMessageRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(new SendMessageCommand(conversationId, request.Content), cancellationToken);
        return Ok(new { id });
    }
}
