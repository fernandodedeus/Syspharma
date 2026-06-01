using SyspharmaApi.Contracts.Auth;

namespace SyspharmaApi.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, string? ip, string? userAgent);
    Task<AuthResponse> LoginAsync(LoginRequest request, string? ip, string? userAgent);
    Task<AuthResponse> RefreshAsync(string refreshToken, string? ip, string? userAgent);
    Task LogoutAsync(string refreshToken);
    Task RevokeAllTokensAsync(int userId);
}
