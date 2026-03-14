namespace Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpireUtc { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

}
