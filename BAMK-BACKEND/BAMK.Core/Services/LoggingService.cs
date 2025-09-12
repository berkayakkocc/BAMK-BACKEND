using BAMK.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace BAMK.Core.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;
        private readonly Dictionary<string, Stopwatch> _operationTimers = new();

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogOperationStart(string operation, object? parameters = null)
        {
            var timer = Stopwatch.StartNew();
            _operationTimers[operation] = timer;

            _logger.LogInformation("Operation started: {Operation} with parameters: {Parameters}", 
                operation, JsonSerializer.Serialize(parameters));
        }

        public void LogOperationEnd(string operation, TimeSpan duration, bool success = true)
        {
            if (_operationTimers.TryGetValue(operation, out var timer))
            {
                timer.Stop();
                _operationTimers.Remove(operation);
            }

            if (success)
            {
                _logger.LogInformation("Operation completed: {Operation} in {Duration}ms", 
                    operation, duration.TotalMilliseconds);
            }
            else
            {
                _logger.LogWarning("Operation failed: {Operation} in {Duration}ms", 
                    operation, duration.TotalMilliseconds);
            }
        }

        public void LogOperationError(string operation, Exception exception, object? parameters = null)
        {
            _logger.LogError(exception, "Error in operation: {Operation} with parameters: {Parameters}", 
                operation, JsonSerializer.Serialize(parameters));
        }

        public void LogPerformance(string operation, TimeSpan duration, string? additionalInfo = null)
        {
            var performanceLevel = GetPerformanceLevel(duration);
            
            switch (performanceLevel)
            {
                case "Excellent":
                    _logger.LogInformation("Performance: {Operation} completed in {Duration}ms - {PerformanceLevel}", 
                        operation, duration.TotalMilliseconds, performanceLevel);
                    break;
                case "Good":
                    _logger.LogInformation("Performance: {Operation} completed in {Duration}ms - {PerformanceLevel}", 
                        operation, duration.TotalMilliseconds, performanceLevel);
                    break;
                case "Slow":
                    _logger.LogWarning("Performance: {Operation} completed in {Duration}ms - {PerformanceLevel}", 
                        operation, duration.TotalMilliseconds, performanceLevel);
                    break;
                case "VerySlow":
                    _logger.LogError("Performance: {Operation} completed in {Duration}ms - {PerformanceLevel}", 
                        operation, duration.TotalMilliseconds, performanceLevel);
                    break;
            }
        }

        public void LogBusinessEvent(string eventName, object? data = null)
        {
            _logger.LogInformation("Business event: {EventName} with data: {Data}", 
                eventName, JsonSerializer.Serialize(data));
        }

        public void LogInfo(string message, object? data = null)
        {
            _logger.LogInformation("{Message} with data: {Data}", message, JsonSerializer.Serialize(data));
        }

        public void LogWarning(string message, object? data = null)
        {
            _logger.LogWarning("{Message} with data: {Data}", message, JsonSerializer.Serialize(data));
        }

        public void LogError(string message, Exception? exception = null, object? data = null)
        {
            if (exception != null)
            {
                _logger.LogError(exception, "{Message} with data: {Data}", message, JsonSerializer.Serialize(data));
            }
            else
            {
                _logger.LogError("{Message} with data: {Data}", message, JsonSerializer.Serialize(data));
            }
        }

        private static string GetPerformanceLevel(TimeSpan duration)
        {
            return duration.TotalMilliseconds switch
            {
                < 100 => "Excellent",
                < 500 => "Good",
                < 2000 => "Slow",
                _ => "VerySlow"
            };
        }
    }
}
