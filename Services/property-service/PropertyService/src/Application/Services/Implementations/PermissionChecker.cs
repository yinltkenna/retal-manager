using EventContracts.Authorization.PermissionsAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PropertyService.src.Application.Interfaces;

namespace PropertyService.src.Application.Services.Implementations
{
    public class PermissionChecker(IHttpContextAccessor httpContextAccessor,
                                 IAuthorizationService authorizationService) : IPermissionChecker
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        public async Task<bool> HasPermissionAsync(string permission)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
                return false;

            var result = await _authorizationService.AuthorizeAsync(user, null, new PermissionRequirement(permission));
            return result.Succeeded;
        }
    }
}
