namespace Domain.Entities;

public sealed class Musician
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public int Experience { get; set; }
    public double BasicSalary { get; set; }

    #region Navigation Properties
    public ICollection<MusicalInstrument> Instruments { get; set; } = new List<MusicalInstrument>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    public ICollection<MusicianActivities> MusicianActivities { get; set; } = new List<MusicianActivities>();
    public ICollection<MusicianPaymentDetail> MusicianPaymentDetails { get; set; } = new List<MusicianPaymentDetail>();

    #endregion

}
