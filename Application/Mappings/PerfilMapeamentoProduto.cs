using AutoMapper;
using DesafioMirante.Application.DTOs.Products;
using DesafioMirante.Domain.Entities;

namespace DesafioMirante.Application.Mappings;

public sealed class PerfilMapeamentoProduto : Profile
{
    public PerfilMapeamentoProduto()
    {
        CreateMap<Produto, RespostaProduto>();
        CreateMap<RequisicaoCriarProduto, Produto>();
        CreateMap<RequisicaoAtualizarProduto, Produto>();
    }
}
