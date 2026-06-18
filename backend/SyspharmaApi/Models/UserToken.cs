using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SyspharmaApi.Models
{
    public partial class UserToken : DbModel
    {
        public int Idusertoken { get; set; }

        public int Iduser { get; set; }

        public string Token { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool Revoked { get; set; }

        public string? OriginIp { get; set; }

        public string? UserAgent { get; set; }

        [ValidateNever]
        public virtual User IduserNavigation { get; set; } = null!;
    }
}
