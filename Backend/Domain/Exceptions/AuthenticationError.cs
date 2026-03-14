using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class AuthenticationError
{
    public static ApiOperationError InvalidCredentials(string message = "Invalid credentials") => ApiOperationError.Failure(nameof(InvalidCredentials), message);
    public static ApiOperationError UserLooked() => ApiOperationError.Failure(nameof(InvalidCredentials), "User Looked");
    public static ApiOperationError RegistrationEmailConflict() => ApiOperationError.Conflict(nameof(RegistrationEmailConflict), $"There are already a user with this email");
    public static ApiOperationError TokenRefreshNotAllowed(string message) => ApiOperationError.Conflict(nameof(RegistrationEmailConflict), message);



}
