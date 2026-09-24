using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Enums;

namespace eFitness.Application.UnitTests.TestHelpers;

public class FakeCurrentUserService : ICurrentUserService
{
    public int? UserId { get; set; }
    public UserRole? Role { get; set; }
    public bool IsAuthenticated => UserId is not null;
}
