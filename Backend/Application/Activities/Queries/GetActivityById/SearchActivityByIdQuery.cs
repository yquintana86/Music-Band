using Application.Abstractions.Messaging;
using Shared.Models.Activity;

namespace Application.Activities.Queries.SearchActivityById;

public sealed record SearchActivityByIdQuery(int Id) : IQuery<ActivityResponse>;
