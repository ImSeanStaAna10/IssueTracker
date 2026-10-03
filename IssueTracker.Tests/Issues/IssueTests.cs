using IssueTracker.Api.Models;
using IssueTracker.Exceptions;
using Xunit;

namespace IssueTracker.Tests.Issues;


public class IssueTests
{

    #region -- START --
    [Fact] public void Start_ShouldChangeStatusToInProgress()
    {
        // Arrange - prepare the objects and variables needed
        var issue = new Issue();
        // Act - run the method of what we test
        issue.Start();
        // Assert - verify if the result is correct
        Assert.Equal("IN_PROGRESS", issue.Status);
    }
    [Fact] public void Start_ShouldThrowException_WhenIssueIsNotOpen()
    {
        var issue = new Issue();

        issue.Start();

        Assert.Throws<InvalidIssueException>(() => issue.Start());
    }
    #endregion

    #region -- RESOLVE --
    [Fact] public void Resolve_ShouldChangeStatusToClose()
    {
        var issue = new Issue();
        issue.Start();
        issue.Resolve();
        Assert.Equal("RESOLVED", issue.Status);
    }
    [Fact] public void Resolve_ShouldThrowException_WhenIssueIsNotInProgress()
    {
        var issue = new Issue();
        Assert.Throws<InvalidIssueException>(() => issue.Resolve());
    }
    #endregion

    #region -- CLOSE --
    [Fact] public void Close_ShouldChangeStatusToResolve()
    {
        var issue = new Issue();

        Action[] actions = { 
            () => issue.Start(), 
            () => issue.Resolve(), 
            () => issue.Close()};

        foreach(var action in actions)
        {
            action();
        }
        Assert.Equal("CLOSED", issue.Status);

    }

    [Fact] public void Close_ShouldThrowException_WhenIssueIsNotResolved()
    {
        var issue = new Issue();

        issue.Start();

        Assert.Throws<InvalidIssueException>(() => issue.Close());
    }
    #endregion
}