using Shared.Common;

namespace Domain.Entities;

public sealed class MusicalInstrument
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Country { get; set; }
    public string? Description { get; set; }
    public InstrumentType Type { get; set; }

    #region Navigation Properties
    public int MusicianId { get; set; }
    public Musician Musician { get; set; } = null!;

    #endregion

}
