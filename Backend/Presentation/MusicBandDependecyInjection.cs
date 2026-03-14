using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public static class MusicBandDependecyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection app)
    {
        return app;
    }
}
