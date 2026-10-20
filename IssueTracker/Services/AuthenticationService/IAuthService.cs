using IssueTracker.Models;

namespace IssueTracker.Services.AuthenticationService;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
}