using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.Application.DTOs.Requests.RoomMaintenance;
using PropertyService.Application.Interfaces;

namespace PropertyService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomMaintenanceController(IRoomMaintenanceService roomMaintenanceService) : ControllerBase
    {
        private readonly IRoomMaintenanceService _roomMaintenanceService = roomMaintenanceService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomMaintenanceService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomMaintenanceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomMaintenanceService.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomMaintenanceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomMaintenanceService.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
