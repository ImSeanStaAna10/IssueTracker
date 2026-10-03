using IssueTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Tests.Helpers;

public static class TestDbContextFactory
{
    public static IssueTrackerDbContext Create()
    {
        var options = new DbContextOptionsBuilder<IssueTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new IssueTrackerDbContext(options);
    }
}