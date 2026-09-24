namespace eFitness.API.Dtos.Chat;

public record StartConversationRequest(int OtherUserId);

public record SendMessageRequest(string Content);
