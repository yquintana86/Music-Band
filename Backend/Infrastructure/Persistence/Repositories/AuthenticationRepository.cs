using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal class AuthenticationRepository : IAuthenticationRepository
{
    private readonly IAppDbContext _appDbContext;

    public AuthenticationRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Add(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        _appDbContext.Users.Add(user);
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await _appDbContext.Users.FindAsync(userId);
        ArgumentNullException.ThrowIfNull(user);

        _appDbContext.Users.Remove(user);
    }

    public async Task<bool> ExistEmailAsync(string email, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        return await _appDbContext.Users.AnyAsync(u => u.Email.CompareTo(email) == 0, cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId,nameof(userId));
        var user = await _appDbContext.Users.FindAsync(userId, cancellationToken);
     
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(email);

        var user = await _appDbContext.Users
                            .Include(u => u.Roles)
                            .FirstOrDefaultAsync(u => u.Email.CompareTo(email) == 0, cancellationToken);
        return user;
    }

    public async Task<User?> GetUserByNameAsync(string userName, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => string.Compare(u.Username, userName, StringComparison.InvariantCulture) == 0, cancellationToken);
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _appDbContext.Users
                        .AsNoTracking().ToListAsync(cancellationToken);
    }

    //public async Task UpdatePasswordAsync(int userId, string passwordHashed)
    //{
    //    var userDb = await GetUserbyIdAsync(userId, default);

    //    if (userDb is null)
    //        throw new ArgumentNullException(nameof(userDb));

    //    userDb.PasswordHash = passwordHashed;
    //}

    //public async Task<bool> UpdateUserAsync(User user)
    //{
    //    if (user is null)
    //        throw new InvalidOperationException("Invalid User Format");

    //    var userDb = await GetUserbyIdAsync(user.Id, default);

    //    if (userDb is null)
    //        return false;

    //    userDb.FirstName = user.FirstName;
    //    userDb.LastName = user.LastName;
    //    userDb.Email = user.Email;
    //    userDb.EmailConfirmed = user.EmailConfirmed;
    //    userDb.IsLooked = user.IsLooked;

    //    _appDbContext.Users.Update(userDb);
    //    await _appDbContext.SaveChangesAsync();
    //    return true;
    //}

    //public async Task UpdateUserLookedAsync(int userId, bool isLooked)
    //{
    //    var user = await GetUserbyIdAsync(userId, default);

    //    if (user is null)
    //        return;

    //    user!.IsLooked = isLooked;
    //    _appDbContext.Users.Update(user);
    //    await _appDbContext.SaveChangesAsync();
    //}
}

