using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IAppDbContext _appDbContext;

    public RefreshTokenRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshToken);
        return await _appDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }

    public void Add(RefreshToken refreshToken)
    {
        if(refreshToken is null ||
            string.IsNullOrEmpty(refreshToken.Token))
         throw new ArgumentNullException(nameof(refreshToken));

        _appDbContext.RefreshTokens.Add(refreshToken);
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        if(refreshToken is null ||
            string.IsNullOrEmpty(refreshToken.Token))
         throw new ArgumentNullException(nameof(refreshToken));

        var tokenDb = await GetByIdAsync(refreshToken.Id);
        ArgumentNullException.ThrowIfNull(tokenDb);

        tokenDb.Token = refreshToken.Token;
        tokenDb.ExpireUtc = refreshToken.ExpireUtc;
    }

    public async Task DeleteAsync(int id)
    {
        var token = await _appDbContext.RefreshTokens.FindAsync(id);
        ArgumentNullException.ThrowIfNull(token);
        this._appDbContext.RefreshTokens.Remove(token);
    }

    public async Task<RefreshToken?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);
        return await _appDbContext.RefreshTokens.FindAsync(id);
    }

    public async Task<bool> IsUserRefreshToken(int userId, string refreshToken)
    {
        var token = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        return token is not null && token.UserId == userId;
    }

}
