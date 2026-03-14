namespace Infrastructure.Authentication;

public sealed class JwtOptions
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
    public int ExpireRefreshUtc { get; init; }
    public int ExpireUtc { get; init; }
}
