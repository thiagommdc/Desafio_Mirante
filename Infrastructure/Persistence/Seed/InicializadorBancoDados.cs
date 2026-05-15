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

        if (!await _contexto.Users.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Realizando carga inicial de usuarios");

            var usuarios = new[]
            {
                new User
                {
                    Nome = "Ana Souza",
                    Email = "ana.souza@mirante.local"
                },
                new User
                {
                    Nome = "Bruno Lima",
                    Email = "bruno.lima@mirante.local"
                },
                new User
                {
                    Nome = "Carla Mendes",
                    Email = "carla.mendes@mirante.local"
                }
            };

            await _contexto.Users.AddRangeAsync(usuarios, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);
        }

        if (await _contexto.Produtos.AnyAsync(cancellationToken))
        {
            if (await _contexto.TaskItems.AnyAsync(cancellationToken))
            {
                return;
            }
        }
        else
        {
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

        if (await _contexto.TaskItems.AnyAsync(cancellationToken))
        {
            return;
        }

        _logger.LogInformation("Realizando carga inicial de tarefas");

        var usuariosAtivos = await _contexto.Users
            .OrderBy(usuario => usuario.Nome)
            .ToListAsync(cancellationToken);

        var tarefas = new[]
        {
            new TaskItem
            {
                Titulo = "Definir backlog inicial do produto",
                Descricao = "Organizar as entregas prioritarias para a primeira sprint.",
                Status = Domain.Enums.TaskItemStatus.Pending,
                Prioridade = Domain.Enums.TaskItemPriority.High,
                UsuarioResponsavelId = usuariosAtivos[0].Id
            },
            new TaskItem
            {
                Titulo = "Configurar pipeline de homologacao",
                Descricao = "Preparar deploy automatizado para o ambiente de homologacao.",
                Status = Domain.Enums.TaskItemStatus.InProgress,
                Prioridade = Domain.Enums.TaskItemPriority.Medium,
                UsuarioResponsavelId = usuariosAtivos[1].Id
            },
            new TaskItem
            {
                Titulo = "Documentar endpoints principais",
                Descricao = "Consolidar a documentacao REST utilizada pelo time consumidor.",
                Status = Domain.Enums.TaskItemStatus.Completed,
                Prioridade = Domain.Enums.TaskItemPriority.Low,
                UsuarioResponsavelId = usuariosAtivos[2].Id
            }
        };

        await _contexto.TaskItems.AddRangeAsync(tarefas, cancellationToken);
        await _contexto.SaveChangesAsync(cancellationToken);

        var comentarios = new[]
        {
            new TaskComment
            {
                TaskItemId = tarefas[0].Id,
                AutorId = usuariosAtivos[1].Id,
                Conteudo = "Backlog refinado com o time de produto pela manha."
            },
            new TaskComment
            {
                TaskItemId = tarefas[1].Id,
                AutorId = usuariosAtivos[0].Id,
                Conteudo = "Pipeline com validacao automatica de migrations pendente."
            }
        };

        await _contexto.TaskComments.AddRangeAsync(comentarios, cancellationToken);
        await _contexto.SaveChangesAsync(cancellationToken);
    }
}
