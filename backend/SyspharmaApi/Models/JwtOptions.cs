namespace SyspharmaApi.Models
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; } = "Syspharma";
        public string Audience { get; set; } = "Syspharma.Clients";
        public string Key { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
    }
}
