using System.ComponentModel.DataAnnotations; 

namespace IssueTracker.Validation;

public class NotWhiteSpaceAttribute: ValidationAttribute
{

    public NotWhiteSpaceAttribute()
    {
        ErrorMessage = "The field cannot be empty or consist only of whitespace.";
    }
    public override bool IsValid(object? value)
    {
        if (value is not string text)
        {
            return false; 
        }

        return !string.IsNullOrWhiteSpace(text);
    }


}