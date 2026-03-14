using Application.Abstractions.Messaging;
using Shared.Common;

namespace Application.Instrument.Commands.UpdateInstrument;

public sealed record UpdateInstrumentCommand(int Id, string Name, string? Country, string? Description,InstrumentType Type, int MusicianId) : ICommand;
