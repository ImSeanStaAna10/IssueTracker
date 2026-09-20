using System.ComponentModel.DataAnnotations;

namespace IssueTracker.DTOs.Issues
{
    public class CreateIssueDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Priority { get; set; } = "MEDIUM";   
        [Required]
        public string Type { get; set; } = "TASK";  
    }
}
