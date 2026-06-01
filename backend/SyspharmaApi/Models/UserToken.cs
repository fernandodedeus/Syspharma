namespace SyspharmaApi.Models
{
    public partial class UserToken
    {
        public int Idusertoken { get; set; }

        public int Iduser { get; set; }

        public string Token { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool Revoked { get; set; }

        public virtual User IduserNavigation { get; set; } = null!;
    }
}
