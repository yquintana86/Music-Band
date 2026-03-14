using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class MusicianError
{
    public static ApiOperationError NotFound(int id) => ApiOperationError.Validation(nameof(NotFound), $"The musician with id: {id} not found");
    public static ApiOperationError InvalidAgeFilter() => ApiOperationError.Validation(nameof(InvalidAgeFilter), "Invalid Age Filter: FromAge field must be greater than ToAge field");
    public static ApiOperationError InvalidId(int id) => ApiOperationError.Validation(nameof(InvalidId), $"Id: {id} invalid");
    public static ApiOperationError InvalidExperienceFilter() => ApiOperationError.Validation(nameof(InvalidExperienceFilter), "Invalid experience filter: FromExperience field must be greater than ToExperience field");
    public static ApiOperationError InvalidBasicSalaryFilter() => ApiOperationError.Validation(nameof(InvalidBasicSalaryFilter), "Invalid basic salary filter: FromBasicSalary field must be greater than ToBasicSalary field");

}
