using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class UserError
{
    public static ApiOperationError InvalidUser() => ApiOperationError.Failure(nameof(InvalidUser), "Invalid User Data");
    public static ApiOperationError UserNotFount() => ApiOperationError.NotFound(nameof(UserNotFount), "User not found");
}
