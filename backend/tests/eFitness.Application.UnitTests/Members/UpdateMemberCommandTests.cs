using eFitness.Application.Common.Exceptions;
using eFitness.Application.Members.Commands.UpdateMember;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eFitness.Application.UnitTests.Members;

public class UpdateMemberCommandTests
{
    private static Member SeedMember(eFitness.Infrastructure.Persistence.ApplicationDbContext context)
    {
        var user = new User { Email = "member@efitness.ba", PasswordHash = "hash", FirstName = "M", LastName = "One", Role = UserRole.Client };
        context.Users.Add(user);
        context.SaveChanges();

        var member = new Member { UserId = user.Id, JoinDate = DateTime.UtcNow };
        context.Members.Add(member);
        context.SaveChanges();

        return member;
    }

    [Fact]
    public async Task Handle_UpdatesMemberFields()
    {
        var context = TestDbContextFactory.Create();
        var member = SeedMember(context);

        var handler = new UpdateMemberCommandHandler(context);
        await handler.Handle(
            new UpdateMemberCommand(member.Id, "061999999", null, "Male", "Street 1", "Sarajevo", "Contact", "062000000", null),
            CancellationToken.None);

        var updated = context.Members.Include(m => m.User).Single(m => m.Id == member.Id);
        updated.User.PhoneNumber.Should().Be("061999999");
        updated.City.Should().Be("Sarajevo");
        updated.EmergencyContactName.Should().Be("Contact");
    }

    [Fact]
    public async Task Handle_ThrowsNotFound_ForUnknownMember()
    {
        var context = TestDbContextFactory.Create();
        var handler = new UpdateMemberCommandHandler(context);

        var act = () => handler.Handle(new UpdateMemberCommand(999, null, null, null, null, null, null, null, null), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ThrowsNotFound_WhenAssignedTrainerDoesNotExist()
    {
        var context = TestDbContextFactory.Create();
        var member = SeedMember(context);

        var handler = new UpdateMemberCommandHandler(context);
        var act = () => handler.Handle(
            new UpdateMemberCommand(member.Id, null, null, null, null, null, null, null, 999),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
