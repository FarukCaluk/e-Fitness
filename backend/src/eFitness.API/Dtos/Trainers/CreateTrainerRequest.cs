namespace eFitness.API.Dtos.Trainers;

public record CreateTrainerRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string Specialization,
    string? Bio,
    int YearsOfExperience,
    decimal HourlyRate);
