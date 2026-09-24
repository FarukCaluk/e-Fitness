namespace eFitness.API.Dtos.Trainers;

public record UpdateTrainerRequest(
    string Specialization,
    string? Bio,
    int YearsOfExperience,
    decimal HourlyRate,
    bool IsAvailable);
