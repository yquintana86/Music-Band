namespace Domain.Entities;

public sealed class PasswordResetToken
{
    public int Id { get; set; }
    public string TokenHash { get; set; } = null!;
    public DateTime ExpiredUtc { get; set; }
    public bool Used { get; set; }
    public DateTime? UsedUtc { get; set; }
    public DateTime CreatedUtc { get; set;}
    public int UserId { get; set; }
    public User User { get; set; } = null!;

}
