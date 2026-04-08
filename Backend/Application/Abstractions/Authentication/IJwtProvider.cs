using Domain.Entities;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace Application.Abstractions.Authentication;

public interface IJwtProvider
{
    Task<string> GenerateAccessTokenAsync(User user);
    string GenerateRefreshToken();
    string HashRawToken(string rawToken);
}
