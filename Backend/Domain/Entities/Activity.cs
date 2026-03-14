namespace Domain.Entities;

public sealed class Activity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Client { get; set; } = null!;
    public string? Description { get; set; }
    public bool International { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }

    #region Navigation Properties
    public ICollection<Musician> Musicians { get; set; } = new List<Musician>();
    public ICollection<MusicianActivities> MusicianActivities { get; set; } = new List<MusicianActivities>();

    #endregion
}
