using BAMK.Domain.Interfaces;
using BAMK.Infrastructure.Data;

namespace BAMK.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BAMKDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(BAMKDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
