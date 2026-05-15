using DesafioMirante.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioMirante.Infrastructure.Persistence.Configurations;

public sealed class ConfiguracaoTaskComment : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.ToTable("TaskComments");

        builder.HasKey(comentario => comentario.Id);

        builder.Property(comentario => comentario.Conteudo)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasOne(comentario => comentario.TaskItem)
            .WithMany(tarefa => tarefa.Comentarios)
            .HasForeignKey(comentario => comentario.TaskItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(comentario => comentario.Autor)
            .WithMany(usuario => usuario.Comentarios)
            .HasForeignKey(comentario => comentario.AutorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(comentario => comentario.TaskItemId);
        builder.HasIndex(comentario => comentario.AutorId);
        builder.HasQueryFilter(comentario => !comentario.Excluido);
    }
}