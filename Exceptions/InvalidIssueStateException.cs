
using Microsoft.AspNetCore.Diagnostics;

namespace IssueTracker.Exceptions;

public class InvalidIssueException: Exception
{
    public InvalidIssueException(string message): base(message)
    {
    }
}