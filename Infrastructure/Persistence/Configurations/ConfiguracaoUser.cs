using DesafioMirante.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioMirante.Infrastructure.Persistence.Configurations;

public sealed class ConfiguracaoUser : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Nome)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(usuario => usuario.Email)
            .HasMaxLength(180)
            .IsRequired();

        builder.HasIndex(usuario => usuario.Email)
            .IsUnique();

        builder.HasQueryFilter(usuario => !usuario.Excluido);
    }
}