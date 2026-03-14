using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Domain.Interfaces;
using PropertyService.Infrastructure.Data;

namespace PropertyService.Infrastructure.Repositories
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
        public async Task<List<T>> ListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;

            // 1. Lọc dữ liệu (nếu có)
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // 2. Sắp xếp dữ liệu (nếu có)
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }

            // 3. Trả về danh sách mặc định
            return await query.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }


        /// <summary>
        /// Soft delete if entity has IsDeleted property, otherwise hard delete.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public void Delete(T entity)
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
        }
    }
}
