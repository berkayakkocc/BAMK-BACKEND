namespace BAMK.Core.Common
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public object? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ApiResponse()
        {
        }

        public ApiResponse(T? data, string message = "İşlem başarılı", bool success = true)
        {
            Data = data;
            Message = message;
            Success = success;
        }

        public static ApiResponse<T> SuccessData(T? data, string message = "İşlem başarılı")
        {
            return new ApiResponse<T>(data, message, true);
        }

        public static ApiResponse<T> ErrorData(string message, object? errors = null)
        {
            return new ApiResponse<T>
            {
                Data = default,
                Message = message,
                Success = false,
                Errors = errors
            };
        }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse() : base()
        {
        }

        public ApiResponse(string message, bool success = true) : base(null, message, success)
        {
        }

        public static new ApiResponse Success(string message = "İşlem başarılı")
        {
            return new ApiResponse(message, true);
        }

        public static new ApiResponse Error(string message, object? errors = null)
        {
            return new ApiResponse(message, false)
            {
                Errors = errors
            };
        }
    }
}
