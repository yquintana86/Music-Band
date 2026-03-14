using MediatR;
using SharedLib.Models.Common;

namespace Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery,ApiOperationResult<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
