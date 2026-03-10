using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.src.Web.Common.Helpers
{
    public class GetUserIdFromToken : ControllerBase
    {
        // Get UserId by Jwt token

        public Guid? UserIdFromToken
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(userId, out var id) ? id : null;
            }
        }
    }
}
