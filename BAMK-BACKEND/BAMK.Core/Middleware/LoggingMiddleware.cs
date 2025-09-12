using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BAMK.Core.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString();

            // Log request
            _logger.LogInformation("Request started: {RequestId} {Method} {Path} from {RemoteIpAddress}",
                requestId, context.Request.Method, context.Request.Path, context.Connection.RemoteIpAddress);

            // Add request ID to response headers
            context.Response.Headers.Add("X-Request-ID", requestId);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                // Log response
                _logger.LogInformation("Request completed: {RequestId} {Method} {Path} {StatusCode} in {ElapsedMilliseconds}ms",
                    requestId, context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
