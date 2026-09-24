using eFitness.Domain.Entities;

namespace eFitness.Application.Trainers;

public record TrainerListItemDto(
    int Id,
    int UserId,
    string FullName,
    string Email,
    string Specialization,
    int YearsOfExperience,
    decimal HourlyRate,
    double Rating,
    bool IsAvailable)
{
    public static TrainerListItemDto FromEntity(Trainer trainer) => new(
        trainer.Id,
        trainer.UserId,
        $"{trainer.User.FirstName} {trainer.User.LastName}",
        trainer.User.Email,
        trainer.Specialization,
        trainer.YearsOfExperience,
        trainer.HourlyRate,
        trainer.Rating,
        trainer.IsAvailable);
}

public record TrainerDetailDto(
    int Id,
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string Specialization,
    string? Bio,
    int YearsOfExperience,
    decimal HourlyRate,
    double Rating,
    bool IsAvailable)
{
    public static TrainerDetailDto FromEntity(Trainer trainer) => new(
        trainer.Id,
        trainer.UserId,
        trainer.User.FirstName,
        trainer.User.LastName,
        trainer.User.Email,
        trainer.User.PhoneNumber,
        trainer.Specialization,
        trainer.Bio,
        trainer.YearsOfExperience,
        trainer.HourlyRate,
        trainer.Rating,
        trainer.IsAvailable);
}
