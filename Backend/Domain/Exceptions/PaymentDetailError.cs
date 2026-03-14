using SharedLib.Models.Common;

namespace Domain.Exceptions;

public class PaymentDetailError
{
    public static ApiOperationError PaymentWithMusicianNotFound() => ApiOperationError.Validation(nameof(PaymentWithMusicianNotFound), $"Payment not done, Musician not found");
    public static ApiOperationError PaymentWithRelatedPlusNotFound() => ApiOperationError.Validation(nameof(PaymentWithRelatedPlusNotFound), $"Payment not done, related Range Plus not found");
    public static ApiOperationError PaymentDateFilterInvalid() => ApiOperationError.Validation(nameof(PaymentDateFilterInvalid), $"Payment date filter not applied, FromDate field must be lower than toDate");
    public static ApiOperationError PaymentSalaryFilterInvalid() => ApiOperationError.Validation(nameof(PaymentSalaryFilterInvalid), $"Payment salary filter not applied, FromSalary field must be lower than toSalary");
    public static ApiOperationError PaymentNotFound() => ApiOperationError.Validation(nameof(PaymentNotFound), $"Payment not found");
}
