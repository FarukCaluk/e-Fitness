using eFitness.Domain.Common;
using eFitness.Domain.Enums;

namespace eFitness.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    public string? ProfileImageUrl { get; set; }

    public Trainer? TrainerProfile { get; set; }
    public Member? MemberProfile { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Announcement> AuthoredAnnouncements { get; set; } = new List<Announcement>();
    public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
}
