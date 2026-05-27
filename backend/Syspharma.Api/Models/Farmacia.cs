namespace Syspharma.Api.Models;

public class Farmacia
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<Produto> Produtos { get; set; } = new();
}
