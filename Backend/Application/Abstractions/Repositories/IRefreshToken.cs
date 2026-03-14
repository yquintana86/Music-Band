using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByIdAsync(int Id);
    Task<RefreshToken?> GetByTokenAsync(string refreshToken);
    Task<bool> IsUserRefreshToken(int userId, string refreshToken);
    void Add(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
    Task DeleteAsync(int Id);

}
