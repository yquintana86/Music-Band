
using Application.Abstractions.Messaging;

namespace Application.Instrument.Commands.DeleteInstrument;

public sealed record DeleteInstrumentCommand(int Id) : ICommand;
