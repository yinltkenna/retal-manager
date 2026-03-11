using TenancyService.src.Infrastructure.Data;
using TenancyService.src.Infrastructure.Repositories.Interfaces;

namespace TenancyService.src.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork(TenancyDbContext db) : IUnitOfWork
    {
        private readonly TenancyDbContext _db = db;
        private readonly Dictionary<Type, object> _repositories = new();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (_repositories.TryGetValue(type, out var existing))
            {
                return (IGenericRepository<T>)existing;
            }

            var repository = new GenericRepository<T>(_db);
            _repositories[type] = repository;
            return repository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
