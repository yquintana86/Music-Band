using SharedLib.Models.Common;

namespace Domain.Exceptions;

public class RoleErrors
{
    public static ApiOperationError RoleNotFoundById(int roleId) => ApiOperationError.NotFound(nameof(RoleNotFoundById), $"The role with id: {roleId} was not found");

}
