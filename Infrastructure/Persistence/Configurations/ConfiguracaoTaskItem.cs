using DesafioMirante.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioMirante.Infrastructure.Persistence.Configurations;

public sealed class ConfiguracaoTaskItem : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("TaskItems");

        builder.HasKey(tarefa => tarefa.Id);

        builder.Property(tarefa => tarefa.Titulo)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(tarefa => tarefa.Descricao)
            .HasMaxLength(2000);

        builder.Property(tarefa => tarefa.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(tarefa => tarefa.Prioridade)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(tarefa => tarefa.UsuarioResponsavel)
            .WithMany(usuario => usuario.TarefasResponsaveis)
            .HasForeignKey(tarefa => tarefa.UsuarioResponsavelId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(tarefa => tarefa.Titulo);
        builder.HasIndex(tarefa => new { tarefa.Status, tarefa.Prioridade });
        builder.HasIndex(tarefa => tarefa.UsuarioResponsavelId);
        builder.HasQueryFilter(tarefa => !tarefa.Excluido);
    }
}