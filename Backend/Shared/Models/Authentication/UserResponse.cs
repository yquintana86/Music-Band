namespace Shared.Models.Authentication;

public sealed class UserResponse
{
    public int Id { get; init; }
    public string Username { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public bool EmailConfirmed { get; init; }
    public bool IsLooked { get; init; }
    public List<string> Roles { get; set; } = new();

}

