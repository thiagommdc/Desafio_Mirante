using DesafioMirante.Api.Contratos;
using FluentValidation.Results;

namespace DesafioMirante.UnitTests.Api;

public sealed class RespostaErroApiFactoryTests
{
    [Fact]
    public void CriarErrosValidacao_DeveAgruparMensagensPorCampo()
    {
        var erros = new[]
        {
            new ValidationFailure("Titulo", "Titulo e obrigatorio."),
            new ValidationFailure("Titulo", "Titulo deve ter no maximo 200 caracteres."),
            new ValidationFailure("Descricao", "Descricao invalida.")
        };

        var resultado = RespostaErroApiFactory.CriarErrosValidacao(erros);

        Assert.Equal(2, resultado.Count);
        Assert.Equal(
            new[] { "Titulo e obrigatorio.", "Titulo deve ter no maximo 200 caracteres." },
            resultado["Titulo"]);
        Assert.Equal(new[] { "Descricao invalida." }, resultado["Descricao"]);
    }
}
