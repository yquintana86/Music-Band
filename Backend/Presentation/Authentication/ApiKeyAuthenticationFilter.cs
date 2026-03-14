using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Presentation.Authentication;

public class ApiKeyAuthenticationFilter : IAuthorizationFilter
{

    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string? apiKey = context.HttpContext.Request.Headers[ApiAuthenticationCredential.ApiKeyHeaderName];

        if (!IsValid(apiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
        };
    }

    private bool IsValid(string? key)
    {
        var apiKey = _configuration.GetValue<string>(ApiAuthenticationCredential.ApiKeySectionName) ?? "";

        return !string.IsNullOrWhiteSpace(key) &&
           !string.IsNullOrWhiteSpace(apiKey) &&
           key == apiKey;

    }

    
}
