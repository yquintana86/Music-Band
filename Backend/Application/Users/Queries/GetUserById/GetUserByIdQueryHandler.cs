using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.Authentication;
using SharedLib.Models.Common;

namespace Application.UserApp.Queries.SearchUserById;

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(IAuthenticationRepository authenticationRepository, ILogger<GetUserByIdQueryHandler> logger)
    {
        _authenticationRepository = authenticationRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == 0)
                return ApiOperationResult.Fail<UserResponse>(UserError.InvalidUser());

            var user = await _authenticationRepository.GetUserByIdAsync(request.Id, cancellationToken);
            if (user is null)
                return ApiOperationResult.Fail<UserResponse>(UserError.UserNotFount());

            return ApiOperationResult.Success(new UserResponse()
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsLooked = user.IsLooked,
                Username = user.Username,
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<UserResponse>(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}
