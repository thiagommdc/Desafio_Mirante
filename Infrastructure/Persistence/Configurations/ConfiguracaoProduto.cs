using DesafioMirante.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioMirante.Infrastructure.Persistence.Configurations;

public sealed class ConfiguracaoProduto : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(produto => produto.Id);

        builder.Property(produto => produto.Nome)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(produto => produto.Sku)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(produto => produto.Descricao)
            .HasMaxLength(500);

        builder.Property(produto => produto.Preco)
            .HasPrecision(18, 2);

        builder.HasIndex(produto => produto.Sku);
        builder.HasIndex(produto => produto.Nome);
        builder.HasQueryFilter(produto => !produto.Excluido);
    }
}
