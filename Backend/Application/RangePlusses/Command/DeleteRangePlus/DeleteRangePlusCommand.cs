using Application.Abstractions.Messaging;

namespace Application.RangePlusses.Command.DeleteRangePlus;

public sealed record DeleteRangePlusCommand(int Id) : ICommand;