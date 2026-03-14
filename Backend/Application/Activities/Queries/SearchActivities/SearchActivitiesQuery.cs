using Application.Abstractions.Messaging;
using Shared.Models.Activity;

namespace Application.Activities.Queries.ListActivities;

public sealed record SearchActivitiesQuery() : IQuery<IEnumerable<ActivityResponse>>;
