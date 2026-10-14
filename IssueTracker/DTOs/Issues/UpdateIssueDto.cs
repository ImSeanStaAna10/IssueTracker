using IssueTracker.Enums;
using System.ComponentModel.DataAnnotations;
using IssueTracker.Validation;

namespace IssueTracker.DTOs.Issues
{
    public class UpdateIssueDto
    {


        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        [Required]
        [NotWhiteSpace]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [NotWhiteSpace]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public IssuePriority Priority { get; set; } = IssuePriority.MEDIUM;

        [Required]
        public IssueType Type { get; set; } = IssueType.TASK;

    }
}
