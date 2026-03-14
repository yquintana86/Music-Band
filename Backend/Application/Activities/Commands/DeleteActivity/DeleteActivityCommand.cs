using Application.Abstractions.Messaging;

namespace Application.Activities.Commands.DeleteActivity;

public sealed record DeleteActivityCommand(int id) : ICommand;

