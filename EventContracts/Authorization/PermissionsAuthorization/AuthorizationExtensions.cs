using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
namespace EventContracts.Authorization.PermissionsAuthorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddPermissionAuthorization(
            this IServiceCollection services,
            params string[] permissions)
        {
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            services.AddAuthorization(options =>
            {
                foreach (var permission in permissions)
                {
                    options.AddPolicy(permission, policy =>
                        policy.Requirements.Add(new PermissionRequirement(permission)));
                }
            });

            return services;
        }
    }
}
