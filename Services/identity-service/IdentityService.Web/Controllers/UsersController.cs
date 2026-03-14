using EventContracts.Authorization.Permissions.IdentityService;
using IdentityService.Application.DTOs.Requests.User;
using IdentityService.Application.Interfaces;
using IdentityService.Web.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UsersController> _logger = logger;

        /// <summary>
        /// Get user by ID with roles and permissions
        /// </summary>
        [Authorize(Policy = UserPermissions.VIEW)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Get user profile
        /// </summary>
        [Authorize(Policy = UserPermissions.VIEW)]
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            var result = await _userService.GetProfileAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Get all users with pagination and search
        /// </summary>
        [Authorize(Policy = UserPermissions.VIEW)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryRequest request)
        {
            var result = await _userService.GetAllUsers(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Create new user (admin only)
        /// </summary>
        [Authorize(Policy = UserPermissions.CREATE)]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var currentUserId = new GetUserIdFromToken().UserIdFromToken;
            // if current user id is null, return 401
            if(currentUserId == null)
                return Unauthorized();

            var result = await _userService.CreateUserAsync(request, currentUserId.Value);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update user profile (self or admin)
        /// </summary>
        [Authorize(Policy = UserPermissions.UPDATE)]
        [HttpPut("profile/{userId}")]
        public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateProfileAsync(userId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update user (admin only)
        /// </summary>
        [Authorize(Policy = UserPermissions.UPDATE)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Soft delete user (admin only)
        /// </summary>
        [Authorize(Policy = UserPermissions.DELETE)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var currentUserId = new GetUserIdFromToken().UserIdFromToken;
            if (currentUserId == null)
                return Unauthorized();

            var result = await _userService.DeleteUserAsync(id, currentUserId.Value);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Lock user out (admin only)
        /// </summary>
        [Authorize(Policy = UserPermissions.LOCK)]
        [HttpPost("{id}/lock")]
        public async Task<IActionResult> LockUser(Guid id)
        {
            var result = await _userService.LockUserAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Unlock user (admin only)
        /// </summary>
        [Authorize(Policy = UserPermissions.LOCK)]
        [HttpPost("{id}/unlock")]
        public async Task<IActionResult> UnlockUser(Guid id)
        {
            var result = await _userService.UnlockUserAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        
    }
}
