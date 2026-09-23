using IssueTracker.Enums;
using IssueTracker.Models;
using IssueTracker.Exceptions;

namespace IssueTracker.Api.Models;

public class Issue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; private set; } = "OPEN";
    public IssuePriority Priority { get; set; } = IssuePriority.MEDIUM;
    public IssueType Type { get; set; } = IssueType.TASK;
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;   
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public void Start()
    {

        if (Status != "OPEN")
        {
            throw new InvalidIssueException(
                "Only an open issue can be started."
            );
        }

        Status = "IN_PROGRESS";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resolve()
    {
        if(Status != "IN_PROGRESS")
        {
            throw new InvalidIssueException(
            "Only an IN_PROGESS issue can be resolve"
                );
        }

        Status = "RESOLVED";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        
        if (Status != "RESOLVED")
        {
            throw new InvalidIssueException(
                "Only RESOLVED issue can be Closed"
                );
        }
        Status = "CLOSED";
        UpdatedAt = DateTime.UtcNow;
    }
}