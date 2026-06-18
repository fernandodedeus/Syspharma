namespace SyspharmaApi.Models
{
    public class Error : DbModel
    {
        public int Iderror { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ErrorAt { get; set; }
    }
}
