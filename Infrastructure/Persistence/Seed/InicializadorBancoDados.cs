using DesafioMirante.Domain.Entities;
using DesafioMirante.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DesafioMirante.Infrastructure.Persistence.Seed;

public sealed class InicializadorBancoDados : Services.IInicializadorBancoDados
{
    private readonly ContextoAplicacao _contexto;
    private readonly ILogger<InicializadorBancoDados> _logger;

    public InicializadorBancoDados(ContextoAplicacao contexto, ILogger<InicializadorBancoDados> logger)
    {
        _contexto = contexto;
        _logger = logger;
    }

    public async Task InicializarAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Aplicando migrations do banco de dados");
        await _contexto.Database.MigrateAsync(cancellationToken);

        if (await _contexto.Produtos.AnyAsync(cancellationToken))
        {
            return;
        }

        _logger.LogInformation("Realizando carga inicial de produtos");

        var produtos = new[]
        {
            new Produto
            {
                Nome = "Notebook Pro 14",
                Sku = "NTB-PRO-14",
                Descricao = "Notebook de alta performance para fluxos de produtividade.",
                Preco = 7499.90m,
                QuantidadeEstoque = 12,
                Ativo = true
            },
            new Produto
            {
                Nome = "Mouse Sem Fio",
                Sku = "MSE-WLS-001",
                Descricao = "Mouse ergonomico sem fio para uso diario em escritorio.",
                Preco = 149.90m,
                QuantidadeEstoque = 50,
                Ativo = true
            },
            new Produto
            {
                Nome = "Monitor 4K 27",
                Sku = "MON-4K-27",
                Descricao = "Monitor de 27 polegadas para times de design e analytics.",
                Preco = 2299.00m,
                QuantidadeEstoque = 8,
                Ativo = true
            }
        };

        await _contexto.Produtos.AddRangeAsync(produtos, cancellationToken);
        await _contexto.SaveChangesAsync(cancellationToken);
    }
}
