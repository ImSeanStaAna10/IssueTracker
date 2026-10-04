using IssueTracker.Api.Models;
using IssueTracker.Enums;
using IssueTracker.Exceptions;
using IssueTracker.Services.IssueService;
using IssueTracker.Tests.Helpers;
using Xunit;

namespace IssueTracker.Tests.Issues;

public class IssueServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrowProjectNotFoundException_WhenProjectDoesNotExist()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var service = new IssueService(context);

        var issue = new Issue
        {
            Title = "Test Issue",
            Description = "Test Description",
            ProjectId = 999,
            Priority = IssuePriority.MEDIUM,
            Type = IssueType.TASK
        };

        // Act & Assert
        await Assert.ThrowsAsync<ProjectNotFoundException>(
            () => service.CreateAsync(issue)
        );
    }
}