using eFitness.Domain.Common;

namespace eFitness.Domain.Entities;

public class Conversation : BaseEntity
{
    public int ParticipantOneId { get; set; }
    public User ParticipantOne { get; set; } = null!;
    public int ParticipantTwoId { get; set; }
    public User ParticipantTwo { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastMessageAt { get; set; }

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}
