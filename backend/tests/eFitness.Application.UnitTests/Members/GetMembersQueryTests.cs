using eFitness.Application.Members.Queries.GetMembers;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.Members;

public class GetMembersQueryTests
{
    private static void SeedMember(eFitness.Infrastructure.Persistence.ApplicationDbContext context, string firstName, string lastName, string email)
    {
        var user = new User { Email = email, PasswordHash = "hash", FirstName = firstName, LastName = lastName, Role = UserRole.Client };
        context.Users.Add(user);
        context.SaveChanges();

        context.Members.Add(new Member { UserId = user.Id, JoinDate = DateTime.UtcNow });
        context.SaveChanges();
    }

    [Fact]
    public async Task Handle_FiltersBySearchTerm_AcrossNameAndEmail()
    {
        var context = TestDbContextFactory.Create();
        SeedMember(context, "Amar", "Kovac", "amar@efitness.ba");
        SeedMember(context, "Selma", "Begic", "selma@efitness.ba");

        var handler = new GetMembersQueryHandler(context);
        var result = await handler.Handle(new GetMembersQuery(1, 10, "amar", null), CancellationToken.None);

        result.TotalCount.Should().Be(1);
        result.Items.Single().FullName.Should().Be("Amar Kovac");
    }

    [Fact]
    public async Task Handle_Paginates_Correctly()
    {
        var context = TestDbContextFactory.Create();
        for (var i = 0; i < 15; i++)
        {
            SeedMember(context, $"First{i}", $"Last{i}", $"member{i}@efitness.ba");
        }

        var handler = new GetMembersQueryHandler(context);
        var firstPage = await handler.Handle(new GetMembersQuery(1, 10, null, null), CancellationToken.None);
        var secondPage = await handler.Handle(new GetMembersQuery(2, 10, null, null), CancellationToken.None);

        firstPage.Items.Should().HaveCount(10);
        firstPage.TotalCount.Should().Be(15);
        firstPage.HasNextPage.Should().BeTrue();
        secondPage.Items.Should().HaveCount(5);
        secondPage.HasNextPage.Should().BeFalse();
    }
}
