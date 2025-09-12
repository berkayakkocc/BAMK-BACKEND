using BAMK.Core.Interfaces;
using BAMK.Core.Middleware;
using BAMK.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace BAMK.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // Add core services
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<ICacheService, CacheService>();

            return services;
        }

        public static IServiceCollection AddCoreMiddleware(this IServiceCollection services)
        {
            // Middleware will be added in Program.cs
            return services;
        }

        public static IApplicationBuilder UseCoreMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }
    }
}
