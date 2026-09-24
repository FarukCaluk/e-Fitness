namespace eFitness.API.Dtos.Members;

public record UpdateMemberRequest(
    string? PhoneNumber,
    DateTime? DateOfBirth,
    string? Gender,
    string? Address,
    string? City,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    int? AssignedTrainerId);
