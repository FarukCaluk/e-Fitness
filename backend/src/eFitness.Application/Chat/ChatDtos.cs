using eFitness.Domain.Entities;

namespace eFitness.Application.Chat;

public record ConversationSummaryDto(
    int Id,
    int OtherParticipantId,
    string OtherParticipantName,
    string? LastMessagePreview,
    DateTime? LastMessageAt,
    int UnreadCount);

public record ChatMessageDto(int Id, int SenderId, string SenderName, string Content, DateTime SentAt, bool IsRead)
{
    public static ChatMessageDto FromEntity(ChatMessage message) => new(
        message.Id,
        message.SenderId,
        $"{message.Sender.FirstName} {message.Sender.LastName}",
        message.Content,
        message.SentAt,
        message.IsRead);
}
