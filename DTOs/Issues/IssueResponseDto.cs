using IssueTracker.Enums;

namespace IssueTracker.DTOs.Issues
{
    public class IssueResponseDto
    {

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public IssuePriority Priority{ get; set; }
        public  IssueType Type { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}
