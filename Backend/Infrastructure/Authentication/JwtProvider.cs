using Application.Abstractions.Authentication;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IPermissionService _permissionService;

    public JwtProvider(IOptions<JwtOptions> _options, IPermissionService permissionService)
    {
        _jwtOptions = _options.Value;
        _permissionService = permissionService;
    }

    private SecurityTokenDescriptor GetTokenDescriptor(List<Claim> claims, DateTime expireUtc)
    {
        var signingcredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expireUtc,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = signingcredentials
        };

        return tokenDescriptor;
    }
    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName)
        };

        HashSet<string> permissions = await _permissionService.GetPermissionAsync(user.Id);

        foreach (var permission in permissions)
        {
            claims.Add(new(CustomClaims.PERMISSION_CLAIM_NAME, permission.ToString()));
        }

        SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(claims, DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireUtc));

        var handler = new JsonWebTokenHandler();
        string tokenValue = handler.CreateToken(tokenDescriptor);

        return tokenValue;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
