using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Members;

public record MemberListItemDto(
    int Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    string? City,
    DateTime JoinDate,
    MembershipStatus? CurrentMembershipStatus,
    string? CurrentPlanName,
    string? AssignedTrainerName)
{
    public static MemberListItemDto FromEntity(Member member)
    {
        var currentMembership = member.Memberships
            .Where(m => m.Status == MembershipStatus.Active)
            .OrderByDescending(m => m.EndDate)
            .FirstOrDefault();

        return new MemberListItemDto(
            member.Id,
            $"{member.User.FirstName} {member.User.LastName}",
            member.User.Email,
            member.User.PhoneNumber,
            member.City,
            member.JoinDate,
            currentMembership?.Status,
            currentMembership?.MembershipPlan.Name,
            member.AssignedTrainer != null ? $"{member.AssignedTrainer.User.FirstName} {member.AssignedTrainer.User.LastName}" : null);
    }
}

public record MemberDetailDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    DateTime? DateOfBirth,
    string? Gender,
    string? Address,
    string? City,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    DateTime JoinDate,
    int? AssignedTrainerId)
{
    public static MemberDetailDto FromEntity(Member member) => new(
        member.Id,
        member.User.FirstName,
        member.User.LastName,
        member.User.Email,
        member.User.PhoneNumber,
        member.DateOfBirth,
        member.Gender,
        member.Address,
        member.City,
        member.EmergencyContactName,
        member.EmergencyContactPhone,
        member.JoinDate,
        member.AssignedTrainerId);
}
