namespace SyspharmaApi.Contracts.User
{
    public sealed record UserDto(
        int Iduser,
        int? Idstore,
        int Role,
        string Name,
        string Email,
        string? Cpf,
        string? Phone,
        bool Active,
        DateTime Createdat,
        string? Profilephotopath);
}
