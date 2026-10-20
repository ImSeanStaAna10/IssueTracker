using System.ComponentModel.DataAnnotations;
using IssueTracker.Validation;
namespace IssueTracker.Models;


public class RegisterRequestDto
{
    [Required]
    [StringLength(150)]
    public string Fullname { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;




    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;


}