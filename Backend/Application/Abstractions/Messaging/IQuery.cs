using MediatR;
using SharedLib.Models.Common;

namespace Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<ApiOperationResult<TResponse>>
{
}

