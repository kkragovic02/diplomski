using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Database.Models;
using Zora.Core.Features.Auth;
using Zora.Core.Features.AuthService;
using Zora.Core.Models;

namespace Zora.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        if (result == null)
            return Unauthorized();

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        var user = await authService.RegisterAsync(request, cancellationToken);
        return Ok(user);
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Za frontend: samo obriše token sa klijenta (nije pravi logout jer JWT je stateless)
        return Ok("Logged out");
    }
}
