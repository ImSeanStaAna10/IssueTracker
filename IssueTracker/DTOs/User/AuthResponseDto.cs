using System.ComponentModel.DataAnnotations;
using IssueTracker.Validation;
namespace IssueTracker.Models;

public class AuthResponseDto
{
   
    public string AccessToken { get; set; } = string.Empty; 
    public string Fullname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(1);  

}