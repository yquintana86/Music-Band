using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class ActivityError
{
    public static ApiOperationError DeleteActivityNotFoundId(int id) => ApiOperationError.Validation(nameof(DeleteActivityNotFoundId), $"Activity cannot be deleted, id: {id} not found");
    public static ApiOperationError UpdateActivityNotFoundId(int id) => ApiOperationError.Validation(nameof(DeleteActivityNotFoundId), $"Activity cannot be updated, id: {id} not found");
    public static ApiOperationError ExistActivityNotFoundId(int id) => ApiOperationError.NotFound(nameof(ExistActivityNotFoundId), $"Activity id: {id} not found");
    public static ApiOperationError InvalidFilterDates() => ApiOperationError.Validation(nameof(InvalidFilterDates), $"Activity filter initial date must be before final date");
    
}
