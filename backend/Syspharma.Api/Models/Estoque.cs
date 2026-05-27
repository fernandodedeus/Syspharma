namespace Syspharma.Api.Models;

public class Estoque
{
    public int Id { get; set; }

    public int FarmaciaId { get; set; }

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;

    public int Quantidade { get; set; }
    public int EstoqueMinimo { get; set; }
}
