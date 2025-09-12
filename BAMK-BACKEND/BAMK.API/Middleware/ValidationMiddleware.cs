using BAMK.Core.Common;
using FluentValidation;
using System.Text.Json;

namespace BAMK.API.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;

        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation hatası: {ValidationErrors}", ex.Errors);
                await HandleValidationExceptionAsync(context, ex);
            }
        }

        private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;

            var validationErrors = ex.Errors.Select(error => new ValidationError(
                error.PropertyName,
                error.ErrorMessage,
                error.AttemptedValue,
                error.ErrorCode
            )).ToList();

            var response = new
            {
                success = false,
                message = "Validation hatası",
                errors = validationErrors
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
