namespace SyspharmaApi.Contracts.Auth;

public sealed record RegisterRequest(
    int? Idstore,
    string Name,
    string Email,
    string Password,
    string? Phone,
    string? Document);
