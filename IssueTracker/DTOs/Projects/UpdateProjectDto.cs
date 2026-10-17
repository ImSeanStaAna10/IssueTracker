using System.ComponentModel.DataAnnotations;
using IssueTracker.Validation;

namespace IssueTracker.Models;

public class UpdateProjectDto
{
    [Required]
    [NotWhiteSpace]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [NotWhiteSpace]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;


    
}   