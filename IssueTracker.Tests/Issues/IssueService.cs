
using IssueTracker.Api.Models;
using IssueTracker.Enums;
using IssueTracker.Exceptions;
using IssueTracker.Models;
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

        const int userId = 1;

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
            () => service.CreateAsync(issue, userId)
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyIssuesOwnedByUser()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        const int userId = 1;
        const int otherUserId = 2;

        var userProject = new Project
        {
            Name = "User Project",
            UserId = userId
        };

        var otherUserProject = new Project
        {
            Name = "Other User Project",
            UserId = otherUserId
        };

        context.Projects.AddRange(userProject, otherUserProject);
        await context.SaveChangesAsync();

        var issue1 = new Issue
        {
            Title = "Test Issue 1",
            Description = "Test Description",
            ProjectId = userProject.Id,
            Priority = IssuePriority.MEDIUM,
            Type = IssueType.TASK
        };

        var issue2 = new Issue
        {
            Title = "Test Issue 2",
            Description = "Test Description 2",
            ProjectId = userProject.Id,
            Priority = IssuePriority.MEDIUM,
            Type = IssueType.TASK
        };

        var otherUserIssue = new Issue
        {
            Title = "Other User Issue",
            Description = "Private issue",
            ProjectId = otherUserProject.Id,
            Priority = IssuePriority.MEDIUM,
            Type = IssueType.TASK
        };

        context.Issues.AddRange(issue1, issue2, otherUserIssue);
        await context.SaveChangesAsync();

        var service = new IssueService(context);

        // Act
        var result = await service.GetAllAsync(userId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Title == "Test Issue 1");
        Assert.Contains(result, i => i.Title == "Test Issue 2");
        Assert.DoesNotContain(result, i => i.Title == "Other User Issue");
    }
}
