using Application.Abstractions.Messaging;
using Domain.Entities;
using Shared.Models.RangePlus;

namespace Application.RangePlusses.Query.SearchRangePlusses;

public sealed record SearchRangePlussesQuery : IQuery<IEnumerable<RangePlusResponse>>;