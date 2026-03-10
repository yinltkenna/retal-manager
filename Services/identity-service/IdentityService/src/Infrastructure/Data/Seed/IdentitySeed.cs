using EventContracts.Authorization.Definitions;
using IdentityService.src.Application.Definitions;
using IdentityService.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityService.src.Infrastructure.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(AppDbContext context, ILogger logger)
        {

            await SeedRoles(context);
            await SeedPermissions(context, logger);
            await SeedInternalPermissions(context);
            await SeedAdminPermissions(context); // Admin full access
            await SeedUsers(context);
            await SeedUserRoles(context);
        }
        private static List<Permission> GetAllPermissions(ILogger logger)
        {
            var result = new List<Permission>();
            var idMap = new Dictionary<Guid, string>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.Name.EndsWith("PermissionDefinitions"));

            foreach (var type in types)
            {
                logger.LogInformation("Scanning: {type}", type.FullName);

                var method = type.GetMethod("Get", BindingFlags.Public | BindingFlags.Static);

                if (method == null)
                {
                    logger.LogWarning("Get() not found in {type}", type.FullName);
                    continue;
                }

                var definitions = method.Invoke(null, null) as IEnumerable<BasePermissionDefinition>;

                if (definitions == null)
                {
                    logger.LogWarning("No permissions returned from {type}", type.FullName);
                    continue;
                }

                foreach (var def in definitions)
                {
                    if (idMap.TryGetValue(def.Id, out string? value))
                    {
                        logger.LogWarning(
                            "Exists ID: {Id}\n" +
                            " - Current permission: {CurrentCode} (in {CurrentClass})\n" +
                            " - Already exists: {ExistingCode} (in {ExistingClass})",
                            def.Id,
                            def.Code, type.FullName,
                            "Unknown code", value);
                        continue;
                    }

                    idMap.Add(def.Id, $"{def.Code} ({type.FullName})");
                    result.Add(new Permission
                    {
                        Id = def.Id,
                        Code = def.Code,
                        Name = def.Name,
                        Group = def.Group,
                        SortOrder = def.SortOrder,
                        Type = "API",
                        IsActive = true
                    });
                }
                logger.LogInformation("Total permissions discovered: {count}", result.Count);

            }
            return result;
        }
        private static async Task SeedPermissions(AppDbContext context, ILogger logger)
        {
            var allDiscovered = GetAllPermissions(logger);
            var existingIds = await context.Permissions
                                .AsNoTracking()
                                .Select(p => p.Id)
                                .ToListAsync();
            logger.LogInformation("Start seeding permissions...");

            var toAdd = allDiscovered
                        .Where(p => !existingIds.Contains(p.Id))
                        .ToList();

            if (toAdd.Count != 0)
            {
                logger.LogInformation("Adding {count} new permissions...", toAdd.Count);
                await context.Permissions.AddRangeAsync(toAdd);
                await context.SaveChangesAsync();
            }

            logger.LogInformation("Seed permissions completed.");
        }
        // Seed Permission internal IdentityService
        private static async Task SeedInternalPermissions(AppDbContext context)
        {
            var internalPermissions = PermissionDefinitions.Get();
            foreach (var def in internalPermissions)
            {
                if (!context.Permissions.Any(x => x.Id == def.Id))
                {
                    context.Permissions.Add(new Permission
                    {
                        Id = def.Id,
                        Code = def.Code,
                        Name = def.Name,
                        Group = def.Group,
                        SortOrder = def.SortOrder,
                        Type = "Internal",
                        IsActive = true
                    });
                }
            }
            await context.SaveChangesAsync();
        }
        private static async Task SeedRoles(AppDbContext context)
        {
            var roles = RoleDefinitions.Get();

            foreach (var role in roles)
            {
                if (!context.Roles.Any(x => x.Id == role.Id))
                {
                    context.Roles.Add(role);
                }
            }

            await context.SaveChangesAsync();
        }
        private static async Task SeedAdminPermissions(AppDbContext context)
        {
            var adminRole = await context.Roles
                .FirstOrDefaultAsync(x => x.Name == "Admin");

            if (adminRole == null) return;

            var permissions = await context.Permissions.ToListAsync();

            foreach (var permission in permissions)
            {
                bool exists = await context.RolePermissions.AnyAsync(x =>
                    x.RoleId == adminRole.Id &&
                    x.PermissionId == permission.Id);

                if (!exists)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = adminRole.Id,
                        PermissionId = permission.Id
                    });
                }
            }

            await context.SaveChangesAsync();
        }
        private static async Task SeedUsers(AppDbContext context)
        {
            var users = UserDefinitions.Get();

            foreach (var u in users)
            {
                if (!context.Users.Any(x => x.Id == u.Id))
                {
                    context.Users.Add(u);
                }
            }

            await context.SaveChangesAsync();
        }
        private static async Task SeedUserRoles(AppDbContext context)
        {
            var defs = UserRoleDefinitions.Get();

            foreach (var ur in defs)
            {
                if (!context.UserRoles.Any(x =>
                    x.UserId == ur.UserId &&
                    x.RoleId == ur.RoleId))
                {
                    context.UserRoles.Add(ur);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
