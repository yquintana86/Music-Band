namespace Shared.Models.Musician;

public sealed class MusicianResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public int Experience { get; set; }
    public double BasicSalary { get; set; }
}
