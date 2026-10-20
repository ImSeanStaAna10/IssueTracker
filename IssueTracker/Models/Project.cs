
using IssueTracker.Api.Models;

namespace IssueTracker.Models;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    // Relationship: One User can have many Projects
    //public int UserId { get; set; }

    //public User User { get; set; } = null!;

    public int? UserId { get; set; }

    public User? User { get; set; }

    // Relationship: One Project can have many Issues
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
