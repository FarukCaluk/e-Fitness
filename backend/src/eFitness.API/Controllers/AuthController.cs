using eFitness.API.Dtos.Auth;
using eFitness.Application.Auth.Commands.Login;
using eFitness.Application.Auth.Commands.Logout;
using eFitness.Application.Auth.Commands.Register;
using eFitness.Application.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefreshTokenCommand = eFitness.Application.Auth.Commands.RefreshToken.RefreshTokenCommand;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private const string RefreshTokenCookieName = "refreshToken";

    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var userId = await _sender.Send(
            new RegisterCommand(request.Email, request.Password, request.FirstName, request.LastName, request.PhoneNumber),
            cancellationToken);

        return CreatedAtAction(nameof(Register), new { id = userId }, new { id = userId });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new LoginCommand(request.Email, request.Password, GetClientIp()),
            cancellationToken);

        SetRefreshTokenCookie(result);

        return Ok(AuthResponse.FromResult(result));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized();
        }

        var result = await _sender.Send(new RefreshTokenCommand(refreshToken, GetClientIp()), cancellationToken);

        SetRefreshTokenCookie(result);

        return Ok(AuthResponse.FromResult(result));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _sender.Send(new LogoutCommand(refreshToken), cancellationToken);
            Response.Cookies.Delete(RefreshTokenCookieName);
        }

        return NoContent();
    }

    private void SetRefreshTokenCookie(AuthResultDto result)
    {
        Response.Cookies.Append(RefreshTokenCookieName, result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = result.RefreshTokenExpiresAt
        });
    }

    private string? GetClientIp() => HttpContext.Connection.RemoteIpAddress?.ToString();
}
