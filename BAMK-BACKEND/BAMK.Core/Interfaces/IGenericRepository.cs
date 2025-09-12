using System.Linq.Expressions;
using BAMK.Core.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace BAMK.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void UpdateAsync(T entity);
        void Remove(T entity);
        void RemoveAsync(T entity);
        void DeleteAsync(T entity);
        Task<int> SaveChangesAsync();
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        
        // Pagination methods
        Task<PagedResult<T>> GetPagedAsync(int page, int pageSize);
        Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>> predicate);
        Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy);
        Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool ascending = true);
        
        // Include methods for related data
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        
        // Transaction methods
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
