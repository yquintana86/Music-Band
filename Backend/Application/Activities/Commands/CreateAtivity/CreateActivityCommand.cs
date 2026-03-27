using Application.Abstractions.Messaging;

namespace Application.Activities.Commands.CreateAtivity;

public sealed record CreateActivityCommand(string Name, string Client, string? Description, bool International, DateTime? Begin, DateTime? End, List<int> MusiciansId) : ICommand;
