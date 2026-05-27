namespace Syspharma.Api.Models;

public class ValidadeProduto
{
    public int Id { get; set; }

    public int FarmaciaId { get; set; }

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;

    public DateTime DataValidade { get; set; }
    public int Quantidade { get; set; }
}
