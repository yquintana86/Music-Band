using Application.Abstractions.Messaging;
using Domain.Exceptions;

namespace Application.Instruments.Commands.DeleteManyInstrument;

public sealed record DeleteManyInstrumentCommand(List<int> instrumentIds) : ICommand;