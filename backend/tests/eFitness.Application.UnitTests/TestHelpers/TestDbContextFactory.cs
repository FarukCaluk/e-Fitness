using eFitness.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.UnitTests.TestHelpers;

public static class TestDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}
