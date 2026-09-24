using eFitness.Application.Common.Exceptions;
using eFitness.Application.MembershipPlans.Commands.CreateMembershipPlan;
using eFitness.Application.MembershipPlans.Commands.DeleteMembershipPlan;
using eFitness.Application.MembershipPlans.Commands.UpdateMembershipPlan;
using eFitness.Application.UnitTests.TestHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eFitness.Application.UnitTests.MembershipPlans;

public class MembershipPlanCommandTests
{
    [Fact]
    public async Task Create_StoresFeatures_InOrder()
    {
        var context = TestDbContextFactory.Create();
        var handler = new CreateMembershipPlanCommandHandler(context);

        var planId = await handler.Handle(
            new CreateMembershipPlanCommand("Standard", "7/7 access", 59m, 30, true, new List<string> { "Gym access", "Personal plan" }),
            CancellationToken.None);

        var plan = context.MembershipPlans.Include(p => p.Features).Single(p => p.Id == planId);
        plan.Features.OrderBy(f => f.OrderIndex).Select(f => f.Description).Should().Equal("Gym access", "Personal plan");
        plan.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Update_ReplacesFeatureList_Completely()
    {
        var context = TestDbContextFactory.Create();
        var createHandler = new CreateMembershipPlanCommandHandler(context);
        var planId = await createHandler.Handle(
            new CreateMembershipPlanCommand("Basic", null, 39m, 30, false, new List<string> { "Old feature" }),
            CancellationToken.None);

        var updateHandler = new UpdateMembershipPlanCommandHandler(context);
        await updateHandler.Handle(
            new UpdateMembershipPlanCommand(planId, "Basic", "Updated", 45m, 30, false, true, new List<string> { "New feature A", "New feature B" }),
            CancellationToken.None);

        var plan = context.MembershipPlans.Include(p => p.Features).Single(p => p.Id == planId);
        plan.Price.Should().Be(45m);
        plan.Features.Select(f => f.Description).Should().BeEquivalentTo("New feature A", "New feature B");
    }

    [Fact]
    public async Task Delete_SoftDeletes_ByMarkingInactive()
    {
        var context = TestDbContextFactory.Create();
        var createHandler = new CreateMembershipPlanCommandHandler(context);
        var planId = await createHandler.Handle(
            new CreateMembershipPlanCommand("Premium", null, 89m, 30, false, new List<string> { "Feature" }),
            CancellationToken.None);

        var deleteHandler = new DeleteMembershipPlanCommandHandler(context);
        await deleteHandler.Handle(new DeleteMembershipPlanCommand(planId), CancellationToken.None);

        var plan = context.MembershipPlans.Single(p => p.Id == planId);
        plan.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_ThrowsNotFound_ForUnknownPlan()
    {
        var context = TestDbContextFactory.Create();
        var handler = new DeleteMembershipPlanCommandHandler(context);

        var act = () => handler.Handle(new DeleteMembershipPlanCommand(999), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
