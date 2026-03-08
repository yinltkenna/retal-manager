using IdentityService.src.Application.DTOs.Requests.Role;
using IdentityService.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.src.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        /// <summary>
        /// Get all roles with pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get role by ID with permissions
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _roleService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Create new role
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleService.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update role
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleService.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Soft delete role
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roleService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Assign role to single user
        /// </summary>
        [HttpPost("assign-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRolesToUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleService.AssignRoleToUser(request.RoleIds, new List<Guid> { request.UserId });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Assign roles to multiple users
        /// </summary>
        [HttpPost("assign-to-users")]
        public async Task<IActionResult> AssignRolesToMultipleUsers([FromBody] AssignRolesToUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleService.AssignRoleToUser(request.RoleIds, new List<Guid> { request.UserId });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Remove roles from users
        /// </summary>
        [HttpPost("remove-from-users")]
        public async Task<IActionResult> RemoveRolesFromUsers([FromBody] RemoveRolesFromUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleService.RemoveRoleFromUser(request.RoleIds, new List<Guid> { request.UserId });
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Assign permissions to roles
        /// </summary>
        [HttpPost("assign-permissions")]
        public async Task<IActionResult> AssignPermissionsToRoles([FromBody] dynamic request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleService.AssignPermissionsToRoles(
                request.PermissionIds.ToObject<List<Guid>>(),
                request.RoleIds.ToObject<List<Guid>>());
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
