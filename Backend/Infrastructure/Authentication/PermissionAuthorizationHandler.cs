using Application.Abstractions.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authentication;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermitionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler( IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        PermitionRequirement requirement)
    {
        HashSet<string> permissions = context.User.Claims.Where(cl => cl.Type == CustomClaims.PERMISSION_CLAIM_NAME).
                                                            Select(cl => cl.Value)
                                                            .ToHashSet();

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
