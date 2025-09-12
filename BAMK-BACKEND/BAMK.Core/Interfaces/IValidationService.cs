using BAMK.Core.Common;

namespace BAMK.Core.Interfaces
{
    public interface IValidationService
    {
        Task<Result> ValidateAsync<T>(T model) where T : class;
        Task<Result> ValidateAsync<T>(IEnumerable<T> models) where T : class;
        Task<Result> ValidateBusinessRulesAsync<T>(T model) where T : class;
        Task<Result> ValidateAsync<T>(T model, string? propertyName = null) where T : class;
    }
}
