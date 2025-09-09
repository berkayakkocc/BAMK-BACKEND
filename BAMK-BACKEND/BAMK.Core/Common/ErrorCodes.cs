namespace BAMK.Core.Common
{
    public enum ErrorCode
    {
        // General errors
        Unknown = 0,
        NotFound = 1,
        ValidationError = 2,
        Unauthorized = 3,
        Forbidden = 4,
        InvalidOperation = 5,
        
        // Network errors
        NetworkError = 100,
        Timeout = 101,
        ServiceUnavailable = 102,
        
        // Database errors
        DatabaseError = 200,
        ConstraintViolation = 201,
        DuplicateKey = 202,
        
        // Business logic errors
        UserNotFound = 300,
        UserAlreadyExists = 301,
        InvalidCredentials = 302,
        AccountLocked = 303,
        EmailNotVerified = 304,
        
        // Validation errors
        RequiredField = 400,
        InvalidFormat = 401,
        OutOfRange = 402,
        TooLong = 403,
        TooShort = 404,
        
        // Configuration errors
        ConfigurationError = 500,
        MissingConfiguration = 501,
        InvalidConfiguration = 502
    }
}
