using Application.Abstractions.Messaging;

namespace Application.Activities.Commands.UpdateActivity;

public sealed record UpdateActivityCommand(int Id, string Name, string Client, string? Description, bool International, DateTime? Begin, DateTime? End, List<int> MusiciansId) : ICommand;
