using SharedLib.Models.Common;
using System.Net.NetworkInformation;

namespace Domain.Exceptions;

public sealed class PasswordResetTokensError
{
    public static ApiOperationError InvalidUserUrl(string email) => ApiOperationError.Validation(nameof(InvalidUserUrl), $"The reset url used doesn't belong to {email}");
    public static ApiOperationError UsedUrl(string username) => ApiOperationError.Validation(nameof(UsedUrl), $"The reset url has been used already to reset password for {username}");
    public static ApiOperationError InvalidToken() => ApiOperationError.Validation(nameof(InvalidToken), $"Invalid or expired token");
}
