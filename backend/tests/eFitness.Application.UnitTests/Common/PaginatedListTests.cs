using eFitness.Application.Common.Models;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.Common;

public class PaginatedListTests
{
    private static eFitness.Infrastructure.Persistence.ApplicationDbContext SeedEquipment(int count)
    {
        var context = TestDbContextFactory.Create();

        for (var i = 1; i <= count; i++)
        {
            context.Equipment.Add(new Domain.Entities.Equipment { Name = $"Item {i:D2}", Category = EquipmentCategory.Strength, Quantity = 1 });
        }

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task CreateAsync_ComputesTotalPages_AndSlicesCorrectly()
    {
        var context = SeedEquipment(25);
        var query = context.Equipment.OrderBy(e => e.Name);

        var page = await PaginatedList<Domain.Entities.Equipment>.CreateAsync(query, pageNumber: 2, pageSize: 10);

        page.Items.Should().HaveCount(10);
        page.Items.First().Name.Should().Be("Item 11");
        page.TotalCount.Should().Be(25);
        page.TotalPages.Should().Be(3);
        page.HasPreviousPage.Should().BeTrue();
        page.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ClampsInvalidPageNumberAndSize()
    {
        var context = SeedEquipment(5);
        var query = context.Equipment.OrderBy(e => e.Name);

        var page = await PaginatedList<Domain.Entities.Equipment>.CreateAsync(query, pageNumber: 0, pageSize: 500);

        page.PageNumber.Should().Be(1);
        page.PageSize.Should().Be(10);
        page.Items.Should().HaveCount(5);
    }
}
