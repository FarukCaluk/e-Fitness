using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Auth.Dtos;

public record UserSummaryDto(int Id, string Email, string FirstName, string LastName, UserRole Role)
{
    public static UserSummaryDto FromEntity(User user) =>
        new(user.Id, user.Email, user.FirstName, user.LastName, user.Role);
}
