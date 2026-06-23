using SyspharmaApi.Contracts.Auth;

namespace SyspharmaApi.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, RequestInfo info);
    Task<AuthResponse> LoginAsync(LoginRequest request, RequestInfo info);
    Task<AuthResponse> RefreshAsync(string refreshToken, RequestInfo info);
    Task LogoutAsync(string refreshToken);
    Task<AuthResponse> SwitchPassAsync(SwitchPassRequest request, RequestInfo info);
    Task RevokeAllTokensAsync(int userId);
}
