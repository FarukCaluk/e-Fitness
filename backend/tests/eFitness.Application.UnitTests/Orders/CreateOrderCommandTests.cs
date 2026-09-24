using eFitness.Application.Common.Exceptions;
using eFitness.Application.Orders;
using eFitness.Application.Orders.Commands.CreateOrder;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eFitness.Application.UnitTests.Orders;

public class CreateOrderCommandTests
{
    private static (eFitness.Infrastructure.Persistence.ApplicationDbContext Context, Member Member, Product Product) SeedContext(int stock = 10)
    {
        var context = TestDbContextFactory.Create();

        var user = new User { Email = "buyer@efitness.ba", PasswordHash = "hash", FirstName = "Buyer", LastName = "One", Role = UserRole.Client };
        context.Users.Add(user);
        context.SaveChanges();

        var member = new Member { UserId = user.Id, JoinDate = DateTime.UtcNow };
        context.Members.Add(member);
        context.SaveChanges();

        var product = new Product { Name = "Whey Protein", Price = 55m, Category = ProductCategory.Supplements, StockQuantity = stock, IsActive = true };
        context.Products.Add(product);
        context.SaveChanges();

        return (context, member, product);
    }

    [Fact]
    public async Task Handle_DecrementsStock_AndRecordsPaymentForOrderTotal()
    {
        var (context, member, product) = SeedContext(stock: 24);
        var currentUser = new FakeCurrentUserService { UserId = member.UserId, Role = UserRole.Client };
        var handler = new CreateOrderCommandHandler(context, currentUser);

        var orderId = await handler.Handle(
            new CreateOrderCommand(new List<OrderItemRequestItem> { new(product.Id, 2) }, "Address 1", PaymentMethod.CreditCard),
            CancellationToken.None);

        var order = context.Orders.Include(o => o.OrderItems).Single(o => o.Id == orderId);
        order.TotalAmount.Should().Be(110m);
        order.Status.Should().Be(OrderStatus.Paid);

        context.Products.Single(p => p.Id == product.Id).StockQuantity.Should().Be(22);

        var payment = context.Payments.Single(p => p.OrderId == orderId);
        payment.Amount.Should().Be(110m);
        payment.Purpose.Should().Be(PaymentPurpose.Order);
        payment.Status.Should().Be(PaymentStatus.Completed);
    }

    [Fact]
    public async Task Handle_ThrowsConflict_WhenStockInsufficient()
    {
        var (context, member, product) = SeedContext(stock: 1);
        var currentUser = new FakeCurrentUserService { UserId = member.UserId, Role = UserRole.Client };
        var handler = new CreateOrderCommandHandler(context, currentUser);

        var act = () => handler.Handle(
            new CreateOrderCommand(new List<OrderItemRequestItem> { new(product.Id, 5) }, null, PaymentMethod.CreditCard),
            CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>();

        context.Products.Single(p => p.Id == product.Id).StockQuantity.Should().Be(1);
        context.Orders.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ThrowsNotFound_ForUnknownProduct()
    {
        var (context, member, _) = SeedContext();
        var currentUser = new FakeCurrentUserService { UserId = member.UserId, Role = UserRole.Client };
        var handler = new CreateOrderCommandHandler(context, currentUser);

        var act = () => handler.Handle(
            new CreateOrderCommand(new List<OrderItemRequestItem> { new(999, 1) }, null, PaymentMethod.CreditCard),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ThrowsForbidden_WhenCurrentUserHasNoMemberProfile()
    {
        var (context, _, product) = SeedContext();
        var currentUser = new FakeCurrentUserService { UserId = 12345, Role = UserRole.Client };
        var handler = new CreateOrderCommandHandler(context, currentUser);

        var act = () => handler.Handle(
            new CreateOrderCommand(new List<OrderItemRequestItem> { new(product.Id, 1) }, null, PaymentMethod.CreditCard),
            CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
