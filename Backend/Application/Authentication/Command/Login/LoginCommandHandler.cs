using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.Common;
using SharedLib.Models.Common;
using Application.Abstractions.DataContext;

namespace Application.Authentication.Command.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, CredentialsResponse>
{

    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(IAuthenticationRepository authenticationRepository, IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider, IRefreshTokenRepository refreshTokenRepository,
        ILogger<LoginCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _authenticationRepository = authenticationRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult<CredentialsResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    { 
        try
        {
            var emailValid = Email.IsValid(request.Email);
            if (!emailValid)
                return ApiOperationResult.Fail<CredentialsResponse>(AuthenticationError.InvalidCredentials("Invalid Email Format"));

            var user = await _authenticationRepository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user is null ||
                !_passwordHasher.Verify(request.Password, user.PasswordHash))
                return ApiOperationResult.Fail<CredentialsResponse>(AuthenticationError.InvalidCredentials());

            if(user.IsLooked)
                return ApiOperationResult.Fail<CredentialsResponse>(AuthenticationError.UserLooked());

            var accessToken = await _jwtProvider.GenerateAccessTokenAsync(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            var refresh = new Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                ExpireUtc = DateTime.UtcNow.AddDays(7),
                Token = refreshToken,
                User = user,
            };
            _refreshTokenRepository.Add(refresh);

            user.IsLogged = true;
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



