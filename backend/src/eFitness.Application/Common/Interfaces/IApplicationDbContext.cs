using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Trainer> Trainers { get; }
    DbSet<Member> Members { get; }
    DbSet<MembershipPlan> MembershipPlans { get; }
    DbSet<MembershipPlanFeature> MembershipPlanFeatures { get; }
    DbSet<Membership> Memberships { get; }
    DbSet<TrainingSession> TrainingSessions { get; }
    DbSet<WorkoutPlan> WorkoutPlans { get; }
    DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<Equipment> Equipment { get; }
    DbSet<Product> Products { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Announcement> Announcements { get; }
    DbSet<Conversation> Conversations { get; }
    DbSet<ChatMessage> ChatMessages { get; }
    DbSet<ProgressLog> ProgressLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
