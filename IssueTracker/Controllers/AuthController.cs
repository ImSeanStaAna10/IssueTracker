
using IssueTracker.Models;
using IssueTracker.Services.AuthenticationService;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
        [FromBody] RegisterRequestDto registerRequest)
    {
        var response = await _authService.RegisterAsync(registerRequest);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginRequestDto loginRequest)
    {
        var response = await _authService.LoginAsync(loginRequest);

        return Ok(response);
    }
}