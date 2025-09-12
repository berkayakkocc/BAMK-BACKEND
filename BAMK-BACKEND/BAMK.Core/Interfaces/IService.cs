using BAMK.Core.Common;

namespace BAMK.Core.Interfaces
{
    public interface IService
    {
        // Base service interface - can be extended by specific services
    }

    public interface IService<TEntity, TDto, TCreateDto, TUpdateDto> : IService
        where TEntity : class
        where TDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        Task<Result<IEnumerable<TDto>>> GetAllAsync();
        Task<Result<TDto?>> GetByIdAsync(int id);
        Task<Result<TDto>> CreateAsync(TCreateDto createDto);
        Task<Result<TDto>> UpdateAsync(int id, TUpdateDto updateDto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<PagedResult<TDto>>> GetPagedAsync(int page, int pageSize);
        Task<Result<PagedResult<TDto>>> GetPagedAsync(int page, int pageSize, System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
    }
}
