using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace BAMK.Core.Services
{
    public abstract class BaseService
    {
        protected readonly ILogger _logger;

        protected BaseService(ILogger logger)
        {
            _logger = logger;
        }

        protected Result<T> HandleResult<T>(T value, string successMessage = "İşlem başarılı")
        {
            return Result<T>.Success(value);
        }

        protected Result HandleResult(string successMessage = "İşlem başarılı")
        {
            return Result.Success();
        }

        protected Result<T> HandleError<T>(string errorMessage, Exception? exception = null)
        {
            if (exception != null)
            {
                _logger.LogError(exception, errorMessage);
            }
            else
            {
                _logger.LogError(errorMessage);
            }

            return Result<T>.Failure(Error.Create(ErrorCode.InvalidOperation, errorMessage));
        }

        protected Result HandleError(string errorMessage, Exception? exception = null)
        {
            if (exception != null)
            {
                _logger.LogError(exception, errorMessage);
            }
            else
            {
                _logger.LogError(errorMessage);
            }

            return Result.Failure(Error.Create(ErrorCode.InvalidOperation, errorMessage));
        }

        protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<T>> operation, string operationName)
        {
            try
            {
                _logger.LogInformation("Starting operation: {OperationName}", operationName);
                var result = await operation();
                _logger.LogInformation("Operation completed successfully: {OperationName}", operationName);
                return Result<T>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Operation failed: {OperationName}", operationName);
                return Result<T>.Failure(Error.Create(ErrorCode.InvalidOperation, $"{operationName} işlemi başarısız oldu"));
            }
        }

        protected async Task<Result> ExecuteAsync(Func<Task> operation, string operationName)
        {
            try
            {
                _logger.LogInformation("Starting operation: {OperationName}", operationName);
                await operation();
                _logger.LogInformation("Operation completed successfully: {OperationName}", operationName);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Operation failed: {OperationName}", operationName);
                return Result.Failure(Error.Create(ErrorCode.InvalidOperation, $"{operationName} işlemi başarısız oldu"));
            }
        }
    }
}
