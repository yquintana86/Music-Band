using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class RangePlusError
{
    public static ApiOperationError ExperienceConflictWithExistingRanges(int minExperience, int maxExperience) => ApiOperationError.Conflict(nameof(ExperienceConflictWithExistingRanges), $"The {minExperience}-{maxExperience} Range values fall between existing ranges min-max experiences");

    public static ApiOperationError PlusFilterInvalid() => ApiOperationError.Validation(nameof(RangePlusError), "Plus filter not applied, From plus must be lower than to plus");
    public static ApiOperationError NotFound(int id) => ApiOperationError.NotFound(nameof(NotFound), $"The range with id:{id} was not found");

    public static ApiOperationError InvalidId(int id) => ApiOperationError.Validation(nameof(InvalidId), $"Operation cannot be executed, Id cannot be less or equal than 0");
}
