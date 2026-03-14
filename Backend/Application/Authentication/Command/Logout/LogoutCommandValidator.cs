using FluentValidation;

namespace Application.Authentication.Command.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(lc => lc.refreshToken).NotEmpty();
        RuleFor(lc => lc.sub).Custom((sub,context) => {
            if (!int.TryParse(sub, out int value) && value <= 0)
                context.AddFailure("Invalid User Id");
        });
    }
}
