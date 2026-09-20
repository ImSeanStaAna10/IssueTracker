using IssueTracker.Enums;

namespace IssueTracker.Api.Models;

public class Issue
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; private set; } = "OPEN";
    public IssuePriority Priority { get; set; } = IssuePriority.MEDIUM;
    public IssueType Type { get; set; } = IssueType.TASK;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public void Start()
    {
        Status = "IN_PROGRESS";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resolve()
    {
        Status = "RESOLVED";
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        Status = "CLOSED";
        UpdatedAt = DateTime.UtcNow;
    }
}