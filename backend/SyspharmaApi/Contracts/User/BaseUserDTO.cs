namespace SyspharmaApi.Contracts.User
{
    public sealed record BaseUserDTO(
        int Iduser,
        string Name,
        string? ProfilePhotoPath,
        bool Active);
}
