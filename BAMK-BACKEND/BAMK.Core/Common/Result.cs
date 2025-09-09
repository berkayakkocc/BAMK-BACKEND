namespace BAMK.Core.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public Error? Error { get; }
        public List<ValidationError> ValidationErrors { get; }
        public string Message => Error?.Message ?? string.Empty;

        private Result(T? value, bool isSuccess, Error? error, List<ValidationError>? validationErrors = null)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException("Success result cannot have an error.");
            if (!isSuccess && error == null)
                throw new InvalidOperationException("Failure result must have an error.");

            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            ValidationErrors = validationErrors ?? new List<ValidationError>();
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result<T> Failure(Error error)
        {
            return new Result<T>(default, false, error);
        }

        public static Result<T> Failure(ErrorCode code, string message, string? field = null, int httpStatusCode = 400)
        {
            return new Result<T>(default, false, Error.Create(code, message, field, httpStatusCode));
        }

        public static Result<T> ValidationFailure(List<ValidationError> validationErrors)
        {
            var error = Error.Create(ErrorCode.ValidationError, "Validation failed", null, 400);
            return new Result<T>(default, false, error, validationErrors);
        }

        public static Result<T> ValidationFailure(ValidationError validationError)
        {
            return ValidationFailure(new List<ValidationError> { validationError });
        }

        public static implicit operator Result<T>(T value) => Success(value);
        public static implicit operator Result<T>(Error error) => Failure(error);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; }
        public List<ValidationError> ValidationErrors { get; }
        public string Message => Error?.Message ?? string.Empty;

        private Result(bool isSuccess, Error? error, List<ValidationError>? validationErrors = null)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException("Success result cannot have an error.");
            if (!isSuccess && error == null)
                throw new InvalidOperationException("Failure result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
            ValidationErrors = validationErrors ?? new List<ValidationError>();
        }

        public static Result Success()
        {
            return new Result(true, null);
        }

        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }

        public static Result Failure(ErrorCode code, string message, string? field = null, int httpStatusCode = 400)
        {
            return new Result(false, Error.Create(code, message, field, httpStatusCode));
        }

        public static Result ValidationFailure(List<ValidationError> validationErrors)
        {
            var error = Error.Create(ErrorCode.ValidationError, "Validation failed", null, 400);
            return new Result(false, error, validationErrors);
        }

        public static Result ValidationFailure(ValidationError validationError)
        {
            return ValidationFailure(new List<ValidationError> { validationError });
        }

        public static implicit operator Result(Error error) => Failure(error);
    }
}
