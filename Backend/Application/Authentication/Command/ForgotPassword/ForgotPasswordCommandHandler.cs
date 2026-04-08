using Application.Abstractions.Authentication;
using Application.Abstractions.DataContext;
using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Utilities;
using Application.Utility;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;
using System.Web;

namespace Application.Authentication.Command.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IEmailService _emailService;
    private readonly IUrlService _urlService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;


    public ForgotPasswordCommandHandler(IPasswordResetTokenRepository tokenRepository, IUnitOfWork unitOfWork, ILogger<ForgotPasswordCommandHandler> logger, IAuthenticationRepository authenticationRepository, IJwtProvider jwtProvider, IEmailService emailService, IUrlService urlService)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _authenticationRepository = authenticationRepository;
        _jwtProvider = jwtProvider;
        _emailService = emailService;
        _urlService = urlService;
    }

    public async Task<ApiOperationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _authenticationRepository.GetUserByEmailAsync(request.email, cancellationToken);
            if (user is null)
            {
                return ApiOperationResult.Fail(AuthenticationError.EmailNotFound());
            }
            if (user.IsLooked)
            {
                return ApiOperationResult.Fail(AuthenticationError.UserLooked());
            }

            var rawToken = _jwtProvider.GenerateRefreshToken();
            var hashToken = _jwtProvider.HashRawToken(rawToken);

            PasswordResetToken token = new PasswordResetToken
            {
                TokenHash = hashToken,
                CreatedUtc = DateTime.UtcNow,
                ExpiredUtc = DateTime.UtcNow.AddMinutes(20),
                UserId = user.Id,
            };

            _tokenRepository.Add(token);            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var htmlBody = EmailTemplates.GetPasswordResetEmailBody($"https://localhost:4200/auth/forgot-password/authentication/forgotpassword?token={HttpUtility.UrlEncode(rawToken)}", 20);
            await _emailService.SendEmailAsync(request.email, "Reset your password", htmlBody);
            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}


