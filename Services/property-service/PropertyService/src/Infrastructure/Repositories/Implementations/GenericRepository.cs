using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PropertyService.src.Infrastructure.Data;
using PropertyService.src.Infrastructure.Repositories.Interfaces;

namespace PropertyService.src.Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T>(PropertyDbContext db) : IGenericRepository<T> where T : class
    {
        private readonly PropertyDbContext _db = db;
        private readonly DbSet<T> _dbSet = db.Set<T>();

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }


        /// <summary>
        /// Soft delete if entity has IsDeleted property, otherwise hard delete.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            // If the entity has an IsDeleted property, set it. Otherwise remove.
            var prop = typeof(T).GetProperty("IsDeleted");
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                prop.SetValue(entity, true);
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }

            return Task.CompletedTask;
        }
    }
}
