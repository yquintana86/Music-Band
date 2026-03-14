namespace Shared.Models.Activity;

public class ActivityResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Client { get; set; } = null!;
    public string? Description { get; set; }
    public bool International { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }

}
