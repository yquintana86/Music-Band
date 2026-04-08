using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

namespace Infrastructure.Persistence.Repositories;

internal sealed class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly AppDbContext _appDbContext;

    public PasswordResetTokenRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Add(PasswordResetToken passwordResetToken)
    {
        this._appDbContext.PasswordResetTokens.Add(passwordResetToken);
    }

    public async Task DeleteAsync(int passwordResetTokenId)
    {
        var token = await GetByIdAsync(passwordResetTokenId, default);
        if (token is null)
            throw new ArgumentException("Token not founded");

        _appDbContext.PasswordResetTokens.Remove(token);
    }

    public async Task<PasswordResetToken?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            await _appDbContext.FindAsync<PasswordResetToken>(id, cancellationToken);

    public async Task<PasswordResetToken?> GetActiveNotUsedByEmailAsync(string email, CancellationToken cancellationToken) =>
            await _appDbContext.PasswordResetTokens
                    .FirstOrDefaultAsync(t => !t.Used &&
                        EF.Functions.Like(email, t.User.Email) &&
                            t.ExpiredUtc < DateTime.UtcNow, cancellationToken);


    public async Task<PasswordResetToken?> GetByHashTokenAsync(string hashToken, CancellationToken cancellationToken) =>
                await _appDbContext.PasswordResetTokens
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => !t.Used &&
                        EF.Functions.Like(hashToken, t.TokenHash) &&
                            t.ExpiredUtc < DateTime.UtcNow, cancellationToken);
}
