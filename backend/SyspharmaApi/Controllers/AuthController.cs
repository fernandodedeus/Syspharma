using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SyspharmaApi.Auth;
using SyspharmaApi.Context;
using SyspharmaApi.Contracts.Auth;
using SyspharmaApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using LoginRequest = SyspharmaApi.Contracts.Auth.LoginRequest;
using RefreshRequest = SyspharmaApi.Contracts.Auth.RefreshRequest;
using RegisterRequest = SyspharmaApi.Contracts.Auth.RegisterRequest;

namespace SyspharmaApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new InvalidOperationException("A senha não pode ser vazia");

            if (request.Password.Length < 6)
                throw new InvalidOperationException("A senha não pode ter menos que 6 caracteres");

            if (request.Password.Length > 50)
                throw new InvalidOperationException("A senha não pode ter mais que 50 caracteres");

            if (request.Email.Length > 300)
                throw new InvalidOperationException("O email não pode ter mais que 300 caracteres");

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();
            var response = await _authService.RegisterAsync(request, ip, userAgent);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest, title: "Erro no cadastro");
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (request.Password.Length > 50)
                throw new UnauthorizedAccessException("A senha não pode ter mais que 50 caracteres");

            if (request.Email.Length > 300)
                throw new UnauthorizedAccessException("O email não pode ter mais que 300 caracteres");

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();
            var response = await _authService.LoginAsync(request, ip, userAgent);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status401Unauthorized, title: "Autenticacao falhou");
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        try
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();

            var response = await _authService.RefreshAsync(
                request.RefreshToken,
                ip,
                userAgent);

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Refresh falhou");
        }
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
    {
        await _authService.LogoutAsync(request.RefreshToken);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(MeResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Me([FromServices] SyspharmaContext db)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                       ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var user = await db.Users.FirstOrDefaultAsync(u => u.Iduser == userId);
        if (user is null) return NotFound();

        return Ok(new MeResponse(
            Id: user.Iduser,
            Idstore: user.Idstore,
            FullName: user.Name,
            Email: user.Email,
            Phone: user.Phone,
            Active: user.Active,
            Document: user.Cpf,
            CreatedAt: user.Createdat));
    }
}