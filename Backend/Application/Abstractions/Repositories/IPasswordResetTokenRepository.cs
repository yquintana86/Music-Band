using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IPasswordResetTokenRepository
{
    Task<PasswordResetToken?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PasswordResetToken?> GetActiveNotUsedByEmailAsync(string email, CancellationToken cancellationToken);
    Task<PasswordResetToken?> GetByHashTokenAsync(string token, CancellationToken cancellationToken);
    void Add(PasswordResetToken passwordResetToken);
    Task DeleteAsync(int passwordResetTokenId);
}
