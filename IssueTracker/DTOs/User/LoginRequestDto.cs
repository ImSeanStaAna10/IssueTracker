using System.ComponentModel.DataAnnotations;
using IssueTracker.Validation;
namespace IssueTracker.Models;


public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}