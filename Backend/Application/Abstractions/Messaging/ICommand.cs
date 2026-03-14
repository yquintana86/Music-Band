using MediatR;
using SharedLib.Models.Common;

namespace Application.Abstractions.Messaging;

public interface ICommand : ICommandBase , IRequest<ApiOperationResult>
{
}

public interface ICommand<TResponse> : ICommandBase , IRequest<ApiOperationResult<TResponse>>
    where TResponse : class
{
    
}

public interface ICommandBase 
{

}
