using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ValidationResult = BAMK.Core.Common.ValidationResult;

namespace BAMK.Core.Services
{
    public class ValidationService : IValidationService
    {
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(ILogger<ValidationService> logger)
        {
            _logger = logger;
        }

        public async Task<Result> ValidateAsync<T>(T model) where T : class
        {
            try
            {
                var validationResult = new ValidationResult();
                var validationContext = new ValidationContext(model);

                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

                if (!isValid)
                {
                    foreach (var validationResultItem in validationResults)
                    {
                        validationResult.AddError(validationResultItem.MemberNames.FirstOrDefault() ?? "General", 
                            validationResultItem.ErrorMessage ?? "Validation error");
                    }
                }

                return validationResult.IsValid 
                    ? Result.Success() 
                    : Result.Failure(Error.Create(ErrorCode.ValidationError, "Validation failed", validationResult.Errors.ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation failed for type {Type}", typeof(T).Name);
                return Result.Failure(Error.Create(ErrorCode.InternalServerError, "Validation işlemi başarısız oldu"));
            }
        }

        public async Task<Result> ValidateAsync<T>(IEnumerable<T> models) where T : class
        {
            try
            {
                var allErrors = new List<ValidationError>();
                var index = 0;

                foreach (var model in models)
                {
                    var validationResult = new ValidationResult();
                    var validationContext = new ValidationContext(model);

                    var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

                    if (!isValid)
                    {
                        foreach (var validationResultItem in validationResults)
                        {
                            allErrors.Add(new ValidationError(
                                $"[{index}].{validationResultItem.MemberNames.FirstOrDefault() ?? "General"}", 
                                validationResultItem.ErrorMessage ?? "Validation error"));
                        }
                    }

                    index++;
                }

                return allErrors.Count == 0 
                    ? Result.Success() 
                    : Result.Failure(Error.Create(ErrorCode.ValidationError, "Validation failed", string.Join("; ", allErrors.Select(e => e.Message))));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Batch validation failed for type {Type}", typeof(T).Name);
                return Result.Failure(Error.Create(ErrorCode.InternalServerError, "Batch validation işlemi başarısız oldu"));
            }
        }

        public async Task<Result> ValidateBusinessRulesAsync<T>(T model) where T : class
        {
            try
            {
                // This method can be overridden in derived classes for specific business rules
                // For now, just return success
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Business rules validation failed for type {Type}", typeof(T).Name);
                return Result.Failure(Error.Create(ErrorCode.InternalServerError, "Business rules validation işlemi başarısız oldu"));
            }
        }

        public async Task<Result> ValidateAsync<T>(T model, string? propertyName = null) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    return await ValidateAsync(model);
                }

                var property = typeof(T).GetProperty(propertyName);
                if (property == null)
                {
                    return Result.Failure(Error.Create(ErrorCode.ValidationError, $"Property {propertyName} not found"));
                }

                var value = property.GetValue(model);
                var validationContext = new ValidationContext(model) { MemberName = propertyName };
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                var isValid = Validator.TryValidateProperty(value, validationContext, validationResults);

                if (!isValid)
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                    return Result.Failure(Error.Create(ErrorCode.ValidationError, string.Join("; ", errors)));
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Property validation failed for {Type}.{Property}", typeof(T).Name, propertyName);
                return Result.Failure(Error.Create(ErrorCode.InternalServerError, "Property validation işlemi başarısız oldu"));
            }
        }
    }
}
