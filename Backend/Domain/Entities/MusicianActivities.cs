using System.Diagnostics;

namespace Domain.Entities;

public sealed class MusicianActivities
{
    public int MusicianId { get; set; }
    public Musician Musician { get; set; } = null!;

    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;

    public decimal SalaryByActivity { get; set; }

}