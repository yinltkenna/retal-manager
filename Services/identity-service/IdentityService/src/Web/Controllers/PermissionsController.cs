using IdentityService.src.Application.DTOs.Requests.Permession;
using IdentityService.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.src.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController(IPermissionService permissionService,
                                       ILogger<PermissionsController> logger) : ControllerBase
    {
        private readonly IPermissionService _permissionService = permissionService;
        private readonly ILogger<PermissionsController> _logger = logger;

        /// <summary>
        /// Get all permissions
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _permissionService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Get permission by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _permissionService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Create new permission
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _permissionService.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update permission
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePermissionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _permissionService.UpdateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Assign permissions to roles
        /// </summary>
        [HttpPost("assign-to-roles")]
        public async Task<IActionResult> AssignToRoles([FromBody] AssignPermissionsToRoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _permissionService.AssignToRoleAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Remove permissions from roles
        /// </summary>
        [HttpPost("remove-from-roles")]
        public async Task<IActionResult> RemoveFromRoles([FromBody] RemovePermissionsToRoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _permissionService.RemoveFromRoleAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
