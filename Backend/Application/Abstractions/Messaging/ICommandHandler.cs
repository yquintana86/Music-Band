using MediatR;
using SharedLib.Models.Common;

namespace Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> :  IRequestHandler<TCommand, ApiOperationResult>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, ApiOperationResult<TResponse>>
    where TCommand : ICommand<TResponse>
    where TResponse : class
{
    
}


