using IssueTracker.Api.Models;
using IssueTracker.Enums;
using IssueTracker.Exceptions;
using IssueTracker.Services.IssueService;
using IssueTracker.Tests.Helpers;
using Xunit;

namespace IssueTracker.Tests.Issues;

public class IssueServiceTests
{
  
    [Fact] public async Task CreateAsync_ShouldThrowProjectNotFoundException_WhenProjectDoesNotExist()
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


    [Fact] public async Task GetAllAsync_ShouldReturnAllIssue()
    {

        //Arrange
       await using var context = TestDbContextFactory.Create();
        
        // Test Data
        var issue1 = new Issue
        {
            Title = "Test Issue 1",
            Description = "Test Description",
            ProjectId = 999,
            Priority = IssuePriority.MEDIUM,
            Type = IssueType.TASK
        };
        var issue2 = new Issue
        {
            Title = "Test Issue 2",
            Description = "Test Description 2",
            ProjectId = 999,
            Priority = IssuePriority.MEDIUM,
            Type = IssueType.TASK
        };


        context.Issues.AddRange(issue1, issue2); // iISTORE SA TEMP DB
        await context.SaveChangesAsync(); // Isave

        var service = new IssueService(context);

        //Act
        var result = await service.GetAllAsync();


        //Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Title == "Test Issue 1");
        Assert.Contains(result, i => i.Title == "Test Issue 2");
    }

}