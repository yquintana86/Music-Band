using Application.Abstractions.Authentication;
using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Authentication.Command.ResetForgottenPassword;

internal sealed class ResetForgottenPasswordHandler : ICommandHandler<ResetForgottenPassword>
{

    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ResetForgottenPasswordHandler> _logger;

    public ResetForgottenPasswordHandler(IPasswordResetTokenRepository tokenRepository, IUnitOfWork unitOfWork, ILogger<ResetForgottenPasswordHandler> logger, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiOperationResult> Handle(ResetForgottenPassword request, CancellationToken cancellationToken)
    {
        try
        {
            var tokendb = await _tokenRepository.GetByHashTokenAsync(_jwtProvider.HashRawToken(request.token), cancellationToken);
            if (tokendb == null)
            {
                return ApiOperationResult.Fail(PasswordResetTokensError.InvalidToken());
            }
            var user = tokendb.User;
            if (string.Compare(user.Email, request.email) != 0)
            {
                return ApiOperationResult.Fail(PasswordResetTokensError.InvalidUserUrl(request.email));
            }

            user.PasswordHash = _passwordHasher.Hash(request.password);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

