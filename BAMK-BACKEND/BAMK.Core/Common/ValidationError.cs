namespace BAMK.Core.Common
{
    public class ValidationError
    {
        public string Field { get; }
        public string Message { get; }
        public object? AttemptedValue { get; }
        public string? ErrorCode { get; }

        public ValidationError(string field, string message, object? attemptedValue = null, string? errorCode = null)
        {
            Field = field;
            Message = message;
            AttemptedValue = attemptedValue;
            ErrorCode = errorCode;
        }

        public static ValidationError Required(string field)
        {
            return new ValidationError(field, $"{field} alanı zorunludur", null, "REQUIRED");
        }

        public static ValidationError InvalidFormat(string field, object? attemptedValue = null)
        {
            return new ValidationError(field, $"{field} alanı geçersiz formatta", attemptedValue, "INVALID_FORMAT");
        }

        public static ValidationError TooLong(string field, int maxLength, object? attemptedValue = null)
        {
            return new ValidationError(field, $"{field} alanı en fazla {maxLength} karakter olabilir", attemptedValue, "TOO_LONG");
        }

        public static ValidationError TooShort(string field, int minLength, object? attemptedValue = null)
        {
            return new ValidationError(field, $"{field} alanı en az {minLength} karakter olmalıdır", attemptedValue, "TOO_SHORT");
        }

        public static ValidationError OutOfRange(string field, object min, object max, object? attemptedValue = null)
        {
            return new ValidationError(field, $"{field} alanı {min} ile {max} arasında olmalıdır", attemptedValue, "OUT_OF_RANGE");
        }

        public static ValidationError Custom(string field, string message, object? attemptedValue = null, string? errorCode = null)
        {
            return new ValidationError(field, message, attemptedValue, errorCode);
        }

        public override string ToString()
        {
            return $"{Field}: {Message}" + (AttemptedValue != null ? $" (Value: {AttemptedValue})" : "");
        }
    }
}
