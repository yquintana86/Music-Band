using Application.Abstractions.Authentication;
using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Common;
using SharedLib.Models.Common;

namespace Application.Authentication.Command.CreateUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(IAuthenticationRepository userRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository, ILogger<RegisterUserCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _authenticationRepository = userRepository;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailValid = Email.IsValid(request.Email);
            if (!emailValid)
                return ApiOperationResult.Fail(AuthenticationError.InvalidCredentials("Invalid Email Format"));


            var user = await _authenticationRepository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user is not null)
                return ApiOperationResult.Fail(AuthenticationError.RegistrationEmailConflict());

            user = new User
            {
                Email = request.Email,
                Username = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var role = await _roleRepository.GetByIdAsync( (int)RoleType.Registered , cancellationToken);
            if (role is null)
                return ApiOperationResult.Fail(RoleErrors.RoleNotFoundById((int)RoleType.Registered));


            user.Roles.Add(role);
            _authenticationRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name, ex.Message, ApiErrorType.Failure);

        }
    }
}
