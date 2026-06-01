namespace SyspharmaApi.Contracts.Auth;

public sealed record AuthResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    int UserId,
    string FullName);
