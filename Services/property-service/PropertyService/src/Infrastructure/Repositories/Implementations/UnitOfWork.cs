using PropertyService.src.Infrastructure.Data;
using PropertyService.src.Infrastructure.Repositories.Interfaces;

namespace PropertyService.src.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork(PropertyDbContext db) : IUnitOfWork
    {
        private readonly PropertyDbContext _db = db;
        private readonly Dictionary<Type, object> _repositories = [];

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (_repositories.TryGetValue(type, out object? value))
            {
                return (IGenericRepository<T>)value;
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
