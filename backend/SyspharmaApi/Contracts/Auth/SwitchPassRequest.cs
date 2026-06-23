namespace SyspharmaApi.Contracts.Auth
{
    public sealed record SwitchPassRequest(
        int Iduser,
        string Oldpass,
        string Newpass);
}
