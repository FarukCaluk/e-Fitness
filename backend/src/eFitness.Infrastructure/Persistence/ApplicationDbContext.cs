using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<MembershipPlanFeature> MembershipPlanFeatures => Set<MembershipPlanFeature>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<WorkoutPlanExercise> WorkoutPlanExercises => Set<WorkoutPlanExercise>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<ProgressLog> ProgressLogs => Set<ProgressLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
