namespace eFitness.Domain.Enums;

public enum UserRole
{
    Admin = 1,
    Trainer = 2,
    Client = 3
}

public enum MembershipStatus
{
    Active = 1,
    Expired = 2,
    Cancelled = 3,
    PendingPayment = 4
}

public enum TrainingSessionStatus
{
    Scheduled = 1,
    Completed = 2,
    Cancelled = 3,
    NoShow = 4
}

public enum PaymentMethod
{
    CreditCard = 1,
    BankTransfer = 2,
    Cash = 3,
    PayPal = 4
}

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4
}

public enum PaymentPurpose
{
    Membership = 1,
    Order = 2,
    TrainingSession = 3
}

public enum OrderStatus
{
    Pending = 1,
    Paid = 2,
    Shipped = 3,
    Completed = 4,
    Cancelled = 5
}

public enum ProductCategory
{
    Supplements = 1,
    Apparel = 2,
    Accessories = 3,
    Equipment = 4
}

public enum EquipmentCategory
{
    Cardio = 1,
    Strength = 2,
    FreeWeights = 3,
    Functional = 4
}

public enum MuscleGroup
{
    Chest = 1,
    Back = 2,
    Shoulders = 3,
    Biceps = 4,
    Triceps = 5,
    Legs = 6,
    Glutes = 7,
    Core = 8,
    FullBody = 9,
    Cardio = 10
}

public enum ExerciseDifficulty
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3
}

public enum NotificationType
{
    Info = 1,
    Warning = 2,
    Success = 3,
    Payment = 4,
    Session = 5,
    Membership = 6
}

public enum AnnouncementSegment
{
    AllMembers = 1,
    BasicPlan = 2,
    StandardPlan = 3,
    PremiumPlan = 4,
    Trainers = 5
}

public enum DayOfWeekPlan
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
}
