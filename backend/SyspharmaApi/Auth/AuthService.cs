using Microsoft.EntityFrameworkCore;
using SyspharmaApi.Context;
using SyspharmaApi.Contracts.Auth;
using SyspharmaApi.Models;

namespace SyspharmaApi.Auth;

public sealed class AuthService(
    SyspharmaContext db,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    ILogger<AuthService> logger) : IAuthService
{
    private const int RefreshTokenExpirationDays = 7;

    private readonly SyspharmaContext _db = db;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, RequestInfo info)
    {
        await ValidateEmailUniqueAsync(request.Email);

        var user = new User
        {
            Idstore = request.Idstore,
            Name = request.Name,
            Email = request.Email.ToLowerInvariant(),
            Phone = request.Phone,
            Pass = _passwordHasher.Hash(request.Password),
            Active = true,
            Cpf = request.Document
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _logger.LogInformation("New user registered: {UserId} {Email}", user.Iduser, user.Email);
        return await GenerateAuthResponseAsync(user, info);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, RequestInfo info)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase));

        if (user is null || !_passwordHasher.Verify(request.Password, user.Pass))
        {
            throw new UnauthorizedAccessException("Email ou senha invalidos.");
        }

        if (!user.Active)
        {
            throw new UnauthorizedAccessException("Conta bloqueada ou inativa.");
        }

        var existingTokens = await _db.UserTokens
            .Where(x =>x.Iduser == user.Iduser && !x.Revoked)
            .ToListAsync();

        existingTokens.ForEach(x => x.Revoked = true);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Login completed: {UserId} {Email}", user.Iduser, user.Email);
        return await GenerateAuthResponseAsync(user, info);
    }

    public async Task<AuthResponse> RefreshAsync(string refreshToken, RequestInfo info)
    {
        var tokenHash = _tokenService.HashRefreshToken(refreshToken);

        var existingToken = await _db.UserTokens
            .FirstOrDefaultAsync(token => token.Token == tokenHash)
            ?? throw new UnauthorizedAccessException("Refresh token inválido.");

        if (existingToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token expirado.");
        }

        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Iduser == existingToken.Iduser)
            ?? throw new UnauthorizedAccessException("Usuário não identificado.");

        return await GenerateAuthResponseRevokingTokens(user, info);
    }    

    public async Task<AuthResponse> SwitchPassAsync(SwitchPassRequest request, RequestInfo info)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Iduser == request.Iduser) 
            ?? throw new InvalidOperationException("Usuário não identificado");

        if (!_passwordHasher.Verify(request.Oldpass, user.Pass))
            throw new UnauthorizedAccessException("A senha antiga está incorreta");

        user.Pass = _passwordHasher.Hash(request.Newpass);
        await _db.SaveChangesAsync();

        return await GenerateAuthResponseRevokingTokens(user, info);
    }

    private async Task<AuthResponse> GenerateAuthResponseRevokingTokens(User user, RequestInfo info)
    {
        await RevokeAllTokensAsync(user.Iduser);

        var newPlainToken = _tokenService.GenerateRefreshToken();
        var newTokenHash = _tokenService.HashRefreshToken(newPlainToken);

        var newRefreshToken = new UserToken
        {
            Iduser = user.Iduser,
            Token = newTokenHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays),
            OriginIp = info.Ip,
            UserAgent = info.UserAgent
        };

        _db.UserTokens.Add(newRefreshToken);
        await _db.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(user);

        return new AuthResponse(
            accessToken,
            newPlainToken,
            900,
            user.Iduser,
            user.Name);
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var tokenHash = _tokenService.HashRefreshToken(refreshToken);
        var existingToken = await _db.UserTokens.FirstOrDefaultAsync(token => token.Token == tokenHash);

        if (existingToken is not null && !existingToken.Revoked)
        {
            existingToken.Revoked = true;
            await _db.SaveChangesAsync();
        }
    }

    public async Task RevokeAllTokensAsync(int userId)
    {
        var activeTokens = await _db.UserTokens
            .Where(token => token.Iduser == userId && !token.Revoked)
            .ToListAsync();

        foreach (var token in activeTokens)
        {
            token.Revoked = true;
        }

        await _db.SaveChangesAsync();
    }

    private async Task ValidateEmailUniqueAsync(string email)
    {
        var exists = await _db.Users.AnyAsync(user => user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        if (exists)
        {
            throw new InvalidOperationException("Email ja cadastrado.");
        }
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(User user, RequestInfo info)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenHash = _tokenService.HashRefreshToken(refreshToken);

        var refreshTokenEntity = new UserToken
        {
            Iduser = user.Iduser,
            Token = refreshTokenHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays),
            OriginIp = info.Ip,
            UserAgent = info.UserAgent
        };

        _db.UserTokens.Add(refreshTokenEntity);
        await _db.SaveChangesAsync();

        return new AuthResponse(
            accessToken,
            refreshToken,
            900,
            user.Iduser,
            user.Name);
    }
}