using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace BAMK.Core.Services
{
    public abstract class GenericService<TEntity, TDto, TCreateDto, TUpdateDto> : BaseService, IService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : class
        where TDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;

        protected GenericService(
            IGenericRepository<TEntity> repository,
            ILogger logger) : base(logger)
        {
            _repository = repository;
        }

        public virtual async Task<Result<IEnumerable<TDto>>> GetAllAsync()
        {
            return await ExecuteAsync(async () =>
            {
                var entities = await _repository.GetAllAsync();
                return MapToDtos(entities);
            }, "GetAllAsync");
        }

        public virtual async Task<Result<TDto?>> GetByIdAsync(int id)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return (TDto?)null;
                }
                return MapToDto(entity);
            }, "GetByIdAsync");
        }

        public virtual async Task<Result<TDto>> CreateAsync(TCreateDto createDto)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = MapToEntity(createDto);
                SetCreateProperties(entity);
                
                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                return MapToDto(entity);
            }, "CreateAsync");
        }

        public virtual async Task<Result<TDto>> UpdateAsync(int id, TUpdateDto updateDto)
        {
            return await ExecuteAsync(async () =>
            {
                var existingEntity = await _repository.GetByIdAsync(id);
                if (existingEntity == null)
                {
                    throw new InvalidOperationException($"Entity with ID {id} not found");
                }

                MapToEntity(updateDto, existingEntity);
                SetUpdateProperties(existingEntity);
                
                _repository.Update(existingEntity);
                await _repository.SaveChangesAsync();

                return MapToDto(existingEntity);
            }, "UpdateAsync");
        }

        public virtual async Task<Result<bool>> DeleteAsync(int id)
        {
            return await ExecuteAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    throw new InvalidOperationException($"Entity with ID {id} not found");
                }

                _repository.Remove(entity);
                await _repository.SaveChangesAsync();

                return true;
            }, "DeleteAsync");
        }

        public virtual async Task<Result<PagedResult<TDto>>> GetPagedAsync(int page, int pageSize)
        {
            return await ExecuteAsync(async () =>
            {
                var pagedResult = await _repository.GetPagedAsync(page, pageSize);
                var dtos = MapToDtos(pagedResult.Items);
                
                return new PagedResult<TDto>(dtos, pagedResult.TotalCount, pagedResult.Page, pagedResult.PageSize);
            }, "GetPagedAsync");
        }

        public virtual async Task<Result<PagedResult<TDto>>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return await ExecuteAsync(async () =>
            {
                var pagedResult = await _repository.GetPagedAsync(page, pageSize, predicate);
                var dtos = MapToDtos(pagedResult.Items);
                
                return new PagedResult<TDto>(dtos, pagedResult.TotalCount, pagedResult.Page, pagedResult.PageSize);
            }, "GetPagedAsync with predicate");
        }

        // Abstract methods to be implemented by derived classes
        protected abstract TDto MapToDto(TEntity entity);
        protected abstract TEntity MapToEntity(TCreateDto createDto);
        protected abstract void MapToEntity(TUpdateDto updateDto, TEntity entity);

        protected virtual IEnumerable<TDto> MapToDtos(IEnumerable<TEntity> entities)
        {
            return entities.Select(MapToDto);
        }

        protected virtual void SetCreateProperties(TEntity entity)
        {
            // Override in derived classes to set specific create properties
        }

        protected virtual void SetUpdateProperties(TEntity entity)
        {
            // Override in derived classes to set specific update properties
        }
    }
}
