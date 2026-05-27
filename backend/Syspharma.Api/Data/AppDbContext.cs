using Microsoft.EntityFrameworkCore;
using Syspharma.Api.Models;

namespace Syspharma.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Farmacia> Farmacias => Set<Farmacia>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Estoque> Estoques => Set<Estoque>();
    public DbSet<ValidadeProduto> ValidadesProdutos => Set<ValidadeProduto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>()
            .HasIndex(p => new { p.FarmaciaId, p.CodigoBarras });

        modelBuilder.Entity<Estoque>()
            .HasIndex(e => new { e.FarmaciaId, e.ProdutoId })
            .IsUnique();
    }
}
