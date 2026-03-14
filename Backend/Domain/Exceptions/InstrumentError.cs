using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class InstrumentError
{
    public static ApiOperationError InvalidId() => ApiOperationError.Validation(nameof(InvalidId), $"Musical Instrument must have an Id greater than 0");
    public static ApiOperationError InvalidInstrumentQtyToSearch() => ApiOperationError.Validation(nameof(InvalidInstrumentQtyToSearch), $"Musical Instrument quantity to search must have a value greater than 0");
    public static ApiOperationError NotFound() => ApiOperationError.Validation(nameof(InvalidId), $"Musical Instrument not found");
    public static ApiOperationError OwnerNotFound(int musicianId) => ApiOperationError.Validation(nameof(OwnerNotFound), $"The id: {musicianId} of the instrument's owner isn't found");
    public static ApiOperationError LastInstrumentToDelete(string ownersName, string instrumentName) => ApiOperationError.Validation(nameof(LastInstrumentToDelete), $"Instrument: {instrumentName} cannot be deleted, is the last of {ownersName}");
    public static ApiOperationError DuplicateOwner() => ApiOperationError.Validation(nameof(DuplicateOwner), $"A musician cannot have more than one instrument of the same type");
    public static ApiOperationError InvalidType() => ApiOperationError.Validation(nameof(InvalidType), $"The instrument type is invalid");

}
