using Application.Abstractions.Authentication;
using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Authentication.Command.Logout;

internal class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(IJwtProvider jwtProvider, ILogger<LogoutCommandHandler> logger,
        IRefreshTokenRepository refreshTokenRepository, IAuthenticationRepository authenticationRepository, IUnitOfWork unitOfWork)
    {
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
        _authenticationRepository = authenticationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tokenDb = await _refreshTokenRepository.GetByTokenAsync(request.refreshToken);
            if (tokenDb == null || tokenDb.ExpireUtc < DateTime.UtcNow)
                return ApiOperationResult.Fail(AuthenticationError.InvalidCredentials("Token Expired"));

            if (tokenDb.UserId != int.Parse(request.sub!))
                return ApiOperationResult.Fail(AuthenticationError.InvalidCredentials("You are not allowed to logout this user session"));

            var user = await this._authenticationRepository.GetUserByIdAsync(tokenDb.UserId, cancellationToken);
            if (user is null)
                return ApiOperationResult.Fail(AuthenticationError.InvalidCredentials("User not Found"));

            //TODO:
            user.IsLogged = false;
            await _refreshTokenRepository.DeleteAsync(tokenDb.Id);
            await this._unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
            
        }
    }
}
