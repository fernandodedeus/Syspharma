namespace SyspharmaApi.Contracts.Auth
{
    public sealed record class RequestInfo(
        string? Ip,
        string? UserAgent);
}
