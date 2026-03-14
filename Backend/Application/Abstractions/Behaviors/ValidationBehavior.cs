using Application.Abstractions.Messaging;
using FluentValidation;
using MediatR;
using SharedLib.Models.Common;
namespace Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
    where TResponse : ApiOperationResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
            if (!_validators.Any())
            {
                return await next();
            }

            var validations = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request)));

            var errors = validations.Where(validatorResult => !validatorResult.IsValid)
                                   .SelectMany(v => v.Errors)
                                   .Select(error => new ApiOperationError(error.PropertyName, error.ErrorMessage, ApiErrorType.Validation))
                                   .ToList();

            if (errors.Any())
            {
                return (TResponse)ApiOperationResult.Fail(errors);
            }

        
        var response = await next();
        return response;
    }
}
