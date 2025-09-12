using BAMK.Core.Common;

namespace BAMK.Core.Interfaces
{
    public interface ILoggingService
    {
        void LogOperationStart(string operation, object? parameters = null);
        void LogOperationEnd(string operation, TimeSpan duration, bool success = true);
        void LogOperationError(string operation, Exception exception, object? parameters = null);
        void LogPerformance(string operation, TimeSpan duration, string? additionalInfo = null);
        void LogBusinessEvent(string eventName, object? data = null);
        void LogInfo(string message, object? data = null);
        void LogWarning(string message, object? data = null);
        void LogError(string message, Exception? exception = null, object? data = null);
    }
}
