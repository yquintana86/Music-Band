using Domain.Entities;
using System.Security.Claims;

namespace Application.Abstractions.Authentication;

public interface IJwtProvider
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();
}
