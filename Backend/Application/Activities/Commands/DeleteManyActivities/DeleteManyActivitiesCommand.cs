using Application.Abstractions.Messaging;
using System.Runtime.CompilerServices;

namespace Application.Activities.Commands.DeleteManyActivities;

public sealed record DeleteManyActivitiesCommand(List<int> ids) : ICommand;