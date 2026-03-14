using Application.Abstractions.Messaging;

namespace Application.RangePlusses.Command.CreateRangePlus;

public sealed record CreateRangePlusCommand(int MinExperience, int MaxExperience, decimal plus) : ICommand;

