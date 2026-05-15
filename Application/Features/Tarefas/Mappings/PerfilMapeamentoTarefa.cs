using AutoMapper;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Domain.Entities;

namespace DesafioMirante.Application.Features.Tarefas.Mappings;

public sealed class PerfilMapeamentoTarefa : Profile
{
    public PerfilMapeamentoTarefa()
    {
        CreateMap<User, RespostaUsuarioTarefa>();

        CreateMap<TaskItem, RespostaTarefa>();
        CreateMap<RequisicaoCriarTarefa, TaskItem>();
        CreateMap<RequisicaoAtualizarTarefa, TaskItem>();

        CreateMap<TaskComment, RespostaComentarioTarefa>();
        CreateMap<RequisicaoCriarComentarioTarefa, TaskComment>();
        CreateMap<RequisicaoAtualizarComentarioTarefa, TaskComment>();
    }
}