using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication;

public class PermitionRequirement : IAuthorizationRequirement
{
    public PermitionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }

}
