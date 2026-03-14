using Application.Abstractions.Messaging;

namespace Application.RangePlusses.Command.UpdateRangePlus;

public sealed record UpdateRangePlusCommand(int Id, int MinExperience, int MaxExperience, decimal plus) : ICommand;
