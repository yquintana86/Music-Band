using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Presentation.Authentication;

public class ApiKeyAuthenticationEndPointFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationEndPointFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string? apikey = context.HttpContext.Request.Headers[ApiAuthenticationCredential.ApiKeyHeaderName];

        if (!IsValid(apikey))
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }

    private bool IsValid(string? key)
    {
        var apiKey = _configuration.GetValue<string>(ApiAuthenticationCredential.ApiKeySectionName) ?? "";

        return !string.IsNullOrWhiteSpace(key) &&
           !string.IsNullOrWhiteSpace(apiKey) &&
           key == apiKey;

    }
}
