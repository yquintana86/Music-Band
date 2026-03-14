
using Application.Abstractions.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authentication;

public class PermissionService : IPermissionService
{
    private readonly IAppDbContext _appDbContext;

    public PermissionService(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<HashSet<string>> GetPermissionAsync(int userId)
    {
        var user = await _appDbContext.Users
                                .Include(u => u.Roles)
                                .ThenInclude(r => r.Permissions)
                                .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return new();

        var userPermissions = user.Roles.SelectMany(u => u.Permissions)
                                   .Select(p => p.Name)
                                   .Distinct()
                                   .ToHashSet();

        return userPermissions;
    }
}
