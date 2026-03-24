using Shared.Common;

namespace Application.Instruments.Queries.Dtos;

public record MusicalInstrumentDTO(int Id, string Name, string? Country, string? Description, InstrumentType Type, int MusicianId, string MusicianName);