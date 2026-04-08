using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class AuthenticationError
{
    public static ApiOperationError InvalidCredentials(string message = "Invalid credentials") => ApiOperationError.Validation(nameof(InvalidCredentials), message);
    public static ApiOperationError EmailNotFound(string message = "Email Not Found") => ApiOperationError.Validation(nameof(EmailNotFound), message);
    public static ApiOperationError UserLooked() => ApiOperationError.Validation(nameof(InvalidCredentials), "User Looked");
    public static ApiOperationError RegistrationEmailConflict() => ApiOperationError.Conflict(nameof(RegistrationEmailConflict), $"There are already a user with this email");
    public static ApiOperationError TokenRefreshNotAllowed(string message) => ApiOperationError.Conflict(nameof(RegistrationEmailConflict), message);



}
