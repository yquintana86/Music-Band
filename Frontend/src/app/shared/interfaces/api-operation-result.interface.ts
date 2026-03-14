export interface APIOperationResultBase {
  isSuccess: boolean;
  errors:    ApiError[] | null;
}
export interface APIOperationResult<T> extends APIOperationResultBase {
  data:      T;
}

export interface ApiError {
  code?:      string;
  message:   string;
  errorType?: number;
}

export enum ApiErrorType
{
    Failure = 0,
    Validation = 5,
    NotFound = 10,
    Conflict = 15,
}
