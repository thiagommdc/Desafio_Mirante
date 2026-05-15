using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;

namespace DesafioMirante.Api.Configuracoes;

public static class ConfiguracaoLoggingExtensions
{
    public static WebApplicationBuilder AdicionarConfiguracaoLogging(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(opcoes =>
        {
            opcoes.AddServerHeader = false;
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Logging.Configure(opcoes =>
        {
            opcoes.ActivityTrackingOptions =
                ActivityTrackingOptions.SpanId |
                ActivityTrackingOptions.TraceId |
                ActivityTrackingOptions.ParentId;
        });

        return builder;
    }
}
