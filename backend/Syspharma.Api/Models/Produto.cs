namespace Syspharma.Api.Models;

public class Produto
{
    public int Id { get; set; }

    public int FarmaciaId { get; set; }
    public Farmacia Farmacia { get; set; } = null!;

    public string Nome { get; set; } = string.Empty;
    public string CodigoBarras { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; }

    public Estoque? Estoque { get; set; }
}
