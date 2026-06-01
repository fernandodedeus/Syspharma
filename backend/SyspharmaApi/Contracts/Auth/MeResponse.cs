namespace SyspharmaApi.Contracts.Auth;

public sealed record MeResponse(
    int Id,
    int? Idstore,
    string FullName,
    string Email,
    string? Phone,
    string? Document,
    bool Active,
    DateTime CreatedAt);
