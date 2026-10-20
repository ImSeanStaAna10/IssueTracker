
using IssueTracker.Api.Models;
namespace IssueTracker.Models;

public class User
{
    public int Id { get; set; }

    public string Fullname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationship: One User can have many Projects
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
