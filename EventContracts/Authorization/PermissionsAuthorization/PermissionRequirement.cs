using Microsoft.AspNetCore.Authorization;
namespace EventContracts.Authorization.PermissionsAuthorization
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
