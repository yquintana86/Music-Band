using Application.Abstractions.Messaging;
using Shared.Models.RangePlus;

namespace Application.RangePlusses.Query.GetRangePlusById;

public sealed record GetRangePlusByIdQuery(int Id) : IQuery<RangePlusResponse>;
