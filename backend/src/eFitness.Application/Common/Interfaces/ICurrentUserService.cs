using eFitness.Domain.Enums;

namespace eFitness.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
    UserRole? Role { get; }
    bool IsAuthenticated { get; }
}
