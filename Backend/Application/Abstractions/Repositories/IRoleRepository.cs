using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int roleId, CancellationToken cancellationToken);

}
