using Application.Abstractions.Authentication;
using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Authentication.Command.RefreshToken;

internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, CredentialsResponse>
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(IAuthenticationRepository authenticationRepository,
        IJwtProvider jwtProvider, IRefreshTokenRepository refreshTokenRepository,
        ILogger<RefreshTokenCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _authenticationRepository = authenticationRepository;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult<CredentialsResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var refreshTokenRequest = request.refreshtoken;
            if (string.IsNullOrWhiteSpace(refreshTokenRequest))
                return ApiOperationResult.Fail<CredentialsResponse>(AuthenticationError.TokenRefreshNotAllowed("Refresh Token required"));

            var tokenDb = await _refreshTokenRepository.GetByTokenAsync(refreshTokenRequest);
            if (tokenDb is null || tokenDb.ExpireUtc < DateTime.UtcNow)
                return ApiOperationResult.Fail<CredentialsResponse>(AuthenticationError.TokenRefreshNotAllowed("Token expired"));

            if (tokenDb.UserId != request.userId)
                return ApiOperationResult.Fail<CredentialsResponse>(AuthenticationError.TokenRefreshNotAllowed("You are not allowed to refresh this token"));
            
            var accessToken = await _jwtProvider.GenerateAccessTokenAsync(tokenDb.User);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            tokenDb.Token = refreshToken;
            tokenDb.ExpireUtc = DateTime.UtcNow.AddDays(7);

            await _refreshTokenRepository.UpdateAsync(tokenDb);
            await _unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success(new CredentialsResponse(accessToken, refreshToken));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<CredentialsResponse>(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }

    }


}


