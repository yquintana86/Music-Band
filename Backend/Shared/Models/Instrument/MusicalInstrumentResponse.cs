using Shared.Common;

namespace Shared.Models.Instrument;

public class MusicalInstrumentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Country { get; set; }
    public string? Description { get; set; }
    public int MusicianId { get; set; }
    public string MusicianName { get; set; } = null!;
    public InstrumentType Type { get; set; }
}
