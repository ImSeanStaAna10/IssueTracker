using IssueTracker.Enums;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.DTOs.Issues
{
    public class UpdateIssueDto
    {

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public IssuePriority Priority { get; set; } = IssuePriority.MEDIUM;

        [Required]
        public IssueType Type { get; set; } = IssueType.TASK;

    }
}
