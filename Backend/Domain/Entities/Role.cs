namespace Domain.Entities;

public sealed class Role 
{

    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? NormalizedName { get; init; }
    public string? ConcurrencyStamp { get; init; }

    #region Navigation Properties

    public ICollection<User> Users { get; init; } = new List<User>();
    public ICollection<Permission> Permissions { get; init; } = new List<Permission>();


    #endregion
}
