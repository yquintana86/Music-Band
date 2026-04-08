using Application.Abstractions.Utilities;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Utilities;

public sealed class UrlService: IUrlService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UrlService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string GetAppRootUrl()
    {
        var request = _contextAccessor.HttpContext?.Request;
        if (request is null) return string.Empty;

        var shceme = request.Scheme; //http or https
        var host = request.Host; //localhost:port or external url

        return $"{shceme}://{host}";
    }
}
