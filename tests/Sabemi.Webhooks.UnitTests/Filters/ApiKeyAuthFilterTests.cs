using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Sabemi.Webhooks.Api.Filters;
using Xunit;

namespace Sabemi.Webhooks.UnitTests.Filters;

public class ApiKeyAuthFilterTests
{
    private static ActionExecutingContext CriarContexto(string? apiKeyHeader)
    {
        var httpContext = new DefaultHttpContext();
        if (apiKeyHeader is not null)
        {
            httpContext.Request.Headers["X-Api-Key"] = apiKeyHeader;
        }

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: null!);
    }

    private static IConfiguration CriarConfiguracao(string apiKeyEsperada)
    {
        var dict = new Dictionary<string, string?> { { "Security:ApiKey", apiKeyEsperada } };
        return new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
    }

    [Fact]
    public async Task OnActionExecutionAsync_SemHeader_DeveRetornar401()
    {
        var filter = new ApiKeyAuthFilter(CriarConfiguracao("chave-correta"));
        var context = CriarContexto(apiKeyHeader: null);
        var proximoChamado = false;

        await filter.OnActionExecutionAsync(context, () =>
        {
            proximoChamado = true;
            return Task.FromResult<ActionExecutedContext>(null!);
        });

        context.Result.Should().BeOfType<UnauthorizedObjectResult>();
        proximoChamado.Should().BeFalse();
    }

    [Fact]
    public async Task OnActionExecutionAsync_ComChaveInvalida_DeveRetornar401()
    {
        var filter = new ApiKeyAuthFilter(CriarConfiguracao("chave-correta"));
        var context = CriarContexto(apiKeyHeader: "chave-errada");
        var proximoChamado = false;

        await filter.OnActionExecutionAsync(context, () =>
        {
            proximoChamado = true;
            return Task.FromResult<ActionExecutedContext>(null!);
        });

        context.Result.Should().BeOfType<UnauthorizedObjectResult>();
        proximoChamado.Should().BeFalse();
    }

    [Fact]
    public async Task OnActionExecutionAsync_ComChaveValida_DevePermitirExecucao()
    {
        var filter = new ApiKeyAuthFilter(CriarConfiguracao("chave-correta"));
        var context = CriarContexto(apiKeyHeader: "chave-correta");
        var proximoChamado = false;

        await filter.OnActionExecutionAsync(context, () =>
        {
            proximoChamado = true;
            return Task.FromResult<ActionExecutedContext>(null!);
        });

        context.Result.Should().BeNull();
        proximoChamado.Should().BeTrue();
    }
}