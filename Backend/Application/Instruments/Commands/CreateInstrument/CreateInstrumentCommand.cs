using Application.Abstractions.Messaging;
using Shared.Common;

namespace Application.Instrument.Commands.CreateInstrument;

public sealed record CreateInstrumentCommand(string Name, string? Country, string? Description, InstrumentType Type, int MusicianId) : ICommand;

