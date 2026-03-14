namespace SharedLib.Models.Common;

public record ApiOperationError
{
    public static ApiOperationError NullReferenceError(Type type, string message = "") => Failure(type.ToString(), $"{type.ToString()} is null");

    public static ApiOperationError NotFound(string code, string message) =>
        new(code, message, ApiErrorType.NotFound);
    public static ApiOperationError Validation(string code, string message) =>
        new(code, message, ApiErrorType.Validation); 
    public static ApiOperationError Conflict(string code, string message) =>
        new(code, message, ApiErrorType.Conflict); 
    public static ApiOperationError Failure(string code, string message) =>
        new(code, message, ApiErrorType.Failure);

    public ApiOperationError(string code, string message, ApiErrorType errorType)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
    }

    public string Code { get; }
    public string Message { get; }
    public ApiErrorType ErrorType { get; }
}


public enum ApiErrorType
{
    Failure = 0,
    Validation = 5,
    NotFound = 10,
    Conflict = 15,
}