namespace SyspharmaApi.Contracts.Auth;

public sealed record MeResponse(
    int Id,
    int? Idstore,
    int Role,
    string FullName,
    string Email,
    string? Phone,
    string? Document,
    bool Active,
    DateTime CreatedAt,
    string? ProfilePhotoPath);
