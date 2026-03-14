using System.Reflection;

namespace Domain.Entities;

public sealed class User
{
    public int Id { get; init; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public bool EmailConfirmed{ get; set; }
    public bool IsLooked { get; set; }
    public bool IsLogged { get; set; }

    #region Navigation Properties

    public ICollection<Role> Roles { get; set; } = new List<Role>();


    #endregion
}
