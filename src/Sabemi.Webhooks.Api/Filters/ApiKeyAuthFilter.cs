using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sabemi.Webhooks.Api.Filters;

public class ApiKeyAuthFilter : IAsyncActionFilter
{
    private const string HeaderName = "X-Api-Key";
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var apiKeyRecebida))
        {
            context.Result = new UnauthorizedObjectResult(new { mensagem = $"Header '{HeaderName}' é obrigatório" });
            return;
        }

        var apiKeyEsperada = _configuration["Security:ApiKey"];

        if (string.IsNullOrEmpty(apiKeyEsperada) || apiKeyRecebida != apiKeyEsperada)
        {
            context.Result = new UnauthorizedObjectResult(new { mensagem = "ApiKey inválida" });
            return;
        }

        await next();
    }
}