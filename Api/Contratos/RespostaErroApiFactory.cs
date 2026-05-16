using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DesafioMirante.Api.Contratos;

public static class RespostaErroApiFactory
{
    public static IReadOnlyDictionary<string, string[]> CriarErrosValidacao(ModelStateDictionary modelState)
    {
        return modelState
            .Where(item => item.Value?.Errors.Count > 0)
            .ToDictionary(
                item => item.Key,
                item => item.Value!.Errors.Select(erro => erro.ErrorMessage).ToArray());
    }

    public static IReadOnlyDictionary<string, string[]> CriarErrosValidacao(IEnumerable<ValidationFailure> erros)
    {
        return erros
            .GroupBy(erro => erro.PropertyName)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Select(erro => erro.ErrorMessage).ToArray());
    }
}
