using FluentValidation;

namespace Application.Authentication.Command.CreateUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(ruc => ruc.Email).NotEmpty();
        RuleFor(ruc => ruc.Password).NotEmpty();
        RuleFor(ruc => ruc.FirstName).NotEmpty();
    }
}
