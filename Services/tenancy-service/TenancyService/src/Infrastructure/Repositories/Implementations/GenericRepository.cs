using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TenancyService.src.Infrastructure.Data;
using TenancyService.src.Infrastructure.Repositories.Interfaces;

namespace TenancyService.src.Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T>(TenancyDbContext db) : IGenericRepository<T> where T : class
    {
        private readonly TenancyDbContext _db = db;

        public async Task AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
        }

        public Task DeleteAsync(T entity)
        {
            // If the entity has an IsDeleted property, soft delete it. Otherwise remove it.
            var prop = typeof(T).GetProperty("IsDeleted");
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                prop.SetValue(entity, true);
                _db.Set<T>().Update(entity);
            }
            else
            {
                _db.Set<T>().Remove(entity);
            }

            return Task.CompletedTask;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null)
        {
            var query = _db.Set<T>().AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            await Task.CompletedTask;
        }
    }
}
