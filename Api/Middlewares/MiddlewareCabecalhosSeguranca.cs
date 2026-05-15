namespace DesafioMirante.Api.Middlewares;

public sealed class MiddlewareCabecalhosSeguranca
{
    private readonly RequestDelegate _next;

    public MiddlewareCabecalhosSeguranca(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
        context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
        context.Response.Headers.TryAdd("Referrer-Policy", "no-referrer");
        context.Response.Headers.TryAdd("X-Permitted-Cross-Domain-Policies", "none");

        await _next(context);
    }
}
