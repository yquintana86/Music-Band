namespace Infrastructure.Services;

public class EmailOptions
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string From { get; init; } = null!;
    public string SmtpServer { get; init; } = null!;
    public int Port { get; init; }
}
