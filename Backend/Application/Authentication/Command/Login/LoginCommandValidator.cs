using FluentValidation;

namespace Application.Authentication.Command.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand> 
{
    public LoginCommandValidator()
    {
        RuleFor(lc => lc.Email).NotEmpty();
        RuleFor(lc => lc.Password).NotEmpty();
        RuleFor(lc => lc.Password).MinimumLength(3);
    }
}



