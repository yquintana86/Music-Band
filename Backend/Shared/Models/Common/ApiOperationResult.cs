using System.Reflection.Metadata.Ecma335;

namespace SharedLib.Models.Common;

public class ApiOperationResult
{
    protected IList<ApiOperationError>? _errors;
    protected bool _success;

    public bool IsSuccess => _success;
    public IReadOnlyList<ApiOperationError>? Errors => 
        _errors is null ? null : new List<ApiOperationError>(_errors);

    public ApiOperationResult()
    {
        _errors = new List<ApiOperationError>();
        _success = false;
    }

    protected ApiOperationResult(IList<ApiOperationError>? errors)
    {
        _errors = errors is null ? null : new List<ApiOperationError>(errors);
        _success = _errors is null || !_errors.Any();
    }

    protected ApiOperationResult(ApiOperationError? error)
    {
        _errors = error is null ? null : new List<ApiOperationError> { error };
        _success = _errors is null || !_errors.Any();
    }

    public static ApiOperationResult Fail(IList<ApiOperationError>? error) => new(error);
    public static ApiOperationResult Fail(ApiOperationError error) => new( error );

    public static ApiOperationResult Fail(string code, string message, ApiErrorType errorType) =>
       Fail(new ApiOperationError(code, message, errorType));

    public static ApiOperationResult<T> Fail<T>(IList<ApiOperationError> errors) =>
        new(errors, default);
    public static ApiOperationResult<T> Fail<T>(ApiOperationError error) =>
       new( error, default);
    public static ApiOperationResult<T> Fail<T>(string code, string message, ApiErrorType errorType) =>
       Fail<T>(new ApiOperationError(code,message, errorType));


    public static ApiOperationResult Success() => new ApiOperationResult((ApiOperationError?)null);
    public static ApiOperationResult<T> Success<T>(T? data)
    {
        return new((ApiOperationError?)null, data);
    }
}


public class ApiOperationResult<T> : ApiOperationResult
{
    private readonly T? _data;

    protected internal ApiOperationResult(IList<ApiOperationError>? errors, T? data) 
        : base (errors) => _data = data;

    protected internal ApiOperationResult(ApiOperationError? error, T? data)
        : base(error) => _data = data;

    public T? Data =>
        IsSuccess ? _data : default;
}
