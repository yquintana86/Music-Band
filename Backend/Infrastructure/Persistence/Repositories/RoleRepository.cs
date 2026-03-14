using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IAppDbContext _appDbContext;

    public RoleRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Role?> GetByIdAsync(int roleId, CancellationToken cancellationToken) =>
        await _appDbContext.Roles.FindAsync(roleId, cancellationToken);
}
