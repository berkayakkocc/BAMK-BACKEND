namespace BAMK.Core.Common
{
    public class Error
    {
        public ErrorCode Code { get; }
        public string Message { get; }
        public string? Field { get; }
        public int HttpStatusCode { get; }
        public Dictionary<string, object>? Metadata { get; }

        private Error(ErrorCode code, string message, string? field = null, int httpStatusCode = 400, Dictionary<string, object>? metadata = null)
        {
            Code = code;
            Message = message;
            Field = field;
            HttpStatusCode = httpStatusCode;
            Metadata = metadata;
        }

        public static Error Create(ErrorCode code, string message, string? field = null, int httpStatusCode = 400, Dictionary<string, object>? metadata = null)
        {
            return new Error(code, message, field, httpStatusCode, metadata);
        }

        public static Error NotFound(string message = "Kaynak bulunamadı", string? field = null)
        {
            return new Error(ErrorCode.NotFound, message, field, 404);
        }

        public static Error Validation(string message, string? field = null)
        {
            return new Error(ErrorCode.ValidationError, message, field, 400);
        }

        public static Error Unauthorized(string message = "Yetkisiz erişim")
        {
            return new Error(ErrorCode.Unauthorized, message, null, 401);
        }

        public static Error Forbidden(string message = "Erişim reddedildi")
        {
            return new Error(ErrorCode.Forbidden, message, null, 403);
        }

        public static Error InvalidOperation(string message)
        {
            return new Error(ErrorCode.InvalidOperation, message, null, 400);
        }

        public static Error Database(string message, Exception? exception = null)
        {
            var metadata = exception != null ? new Dictionary<string, object> { ["Exception"] = exception.Message } : null;
            return new Error(ErrorCode.DatabaseError, message, null, 500, metadata);
        }

        public static Error UserNotFound(string email)
        {
            return new Error(ErrorCode.UserNotFound, $"Kullanıcı bulunamadı: {email}", "email", 404);
        }

        public static Error UserAlreadyExists(string email)
        {
            return new Error(ErrorCode.UserAlreadyExists, $"Bu email adresi zaten kullanılıyor: {email}", "email", 409);
        }

        public static Error InvalidCredentials()
        {
            return new Error(ErrorCode.InvalidCredentials, "Geçersiz email veya şifre", null, 401);
        }

        public override string ToString()
        {
            return $"{Code}: {Message}" + (Field != null ? $" (Field: {Field})" : "");
        }
    }
}
