using IdentityService.src.Domain.Entities;
using IdentityService.src.Infrastructure.Data;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.src.Infrastructure.Repositories.Implementations
{
    public class UserRepository(
        AppDbContext db
        ) : IUserRepository
    {
        private readonly AppDbContext _db = db;
        
        public Task<IQueryable<User>> GetAllUsers()
        {
            return Task.FromResult(_db.Users.AsQueryable());
        }
        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _db.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<Guid>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _db.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
        }

    }
}