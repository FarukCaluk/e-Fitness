export enum MembershipStatus {
  Active = 'Active',
  Expired = 'Expired',
  Cancelled = 'Cancelled',
  PendingPayment = 'PendingPayment'
}

export enum TrainingSessionStatus {
  Scheduled = 'Scheduled',
  Completed = 'Completed',
  Cancelled = 'Cancelled',
  NoShow = 'NoShow'
}

export enum PaymentMethod {
  CreditCard = 'CreditCard',
  BankTransfer = 'BankTransfer',
  Cash = 'Cash',
  PayPal = 'PayPal'
}

export enum PaymentStatus {
  Pending = 'Pending',
  Completed = 'Completed',
  Failed = 'Failed',
  Refunded = 'Refunded'
}

export enum PaymentPurpose {
  Membership = 'Membership',
  Order = 'Order',
  TrainingSession = 'TrainingSession'
}

export enum OrderStatus {
  Pending = 'Pending',
  Paid = 'Paid',
  Shipped = 'Shipped',
  Completed = 'Completed',
  Cancelled = 'Cancelled'
}

export enum ProductCategory {
  Supplements = 'Supplements',
  Apparel = 'Apparel',
  Accessories = 'Accessories',
  Equipment = 'Equipment'
}

export enum EquipmentCategory {
  Cardio = 'Cardio',
  Strength = 'Strength',
  FreeWeights = 'FreeWeights',
  Functional = 'Functional'
}

export enum MuscleGroup {
  Chest = 'Chest',
  Back = 'Back',
  Shoulders = 'Shoulders',
  Biceps = 'Biceps',
  Triceps = 'Triceps',
  Legs = 'Legs',
  Glutes = 'Glutes',
  Core = 'Core',
  FullBody = 'FullBody',
  Cardio = 'Cardio'
}

export enum ExerciseDifficulty {
  Beginner = 'Beginner',
  Intermediate = 'Intermediate',
  Advanced = 'Advanced'
}

export enum NotificationType {
  Info = 'Info',
  Warning = 'Warning',
  Success = 'Success',
  Payment = 'Payment',
  Session = 'Session',
  Membership = 'Membership'
}

export enum AnnouncementSegment {
  AllMembers = 'AllMembers',
  BasicPlan = 'BasicPlan',
  StandardPlan = 'StandardPlan',
  PremiumPlan = 'PremiumPlan',
  Trainers = 'Trainers'
}

export enum DayOfWeekPlan {
  Monday = 'Monday',
  Tuesday = 'Tuesday',
  Wednesday = 'Wednesday',
  Thursday = 'Thursday',
  Friday = 'Friday',
  Saturday = 'Saturday',
  Sunday = 'Sunday'
}
