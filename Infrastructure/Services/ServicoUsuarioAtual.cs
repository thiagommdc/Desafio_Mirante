using DesafioMirante.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace DesafioMirante.Infrastructure.Services;

public sealed class ServicoUsuarioAtual : IServicoUsuarioAtual
{
    private readonly IHttpContextAccessor _acessadorContextoHttp;

    public ServicoUsuarioAtual(IHttpContextAccessor acessadorContextoHttp)
    {
        _acessadorContextoHttp = acessadorContextoHttp;
    }

    public string ObterIdentificadorUsuarioAtual()
    {
        var nomeUsuario = _acessadorContextoHttp.HttpContext?.User?.Identity?.Name;

        if (!string.IsNullOrWhiteSpace(nomeUsuario))
        {
            return nomeUsuario;
        }

        return "sistema";
    }
}
