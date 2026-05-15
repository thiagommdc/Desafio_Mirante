using AutoMapper;
using DesafioMirante.Application.Features.Produtos.DTOs;
using DesafioMirante.Domain.Entities;

namespace DesafioMirante.Application.Features.Produtos.Mappings;

public sealed class PerfilMapeamentoProduto : Profile
{
    public PerfilMapeamentoProduto()
    {
        CreateMap<Produto, RespostaProduto>();
        CreateMap<RequisicaoCriarProduto, Produto>();
        CreateMap<RequisicaoAtualizarProduto, Produto>();
    }
}
