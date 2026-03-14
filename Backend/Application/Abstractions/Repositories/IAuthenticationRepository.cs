using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IAuthenticationRepository
{
    void Add(User user);
    Task DeleteAsync(int userId);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetUserByNameAsync(string userName, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> ExistEmailAsync(string email, CancellationToken cancellationToken);
 
}
