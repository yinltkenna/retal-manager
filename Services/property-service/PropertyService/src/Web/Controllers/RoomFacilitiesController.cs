using PropertyService.src.Application.DTOs.Requests.RoomFacility;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertyService.src.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class RoomFacilitiesController(IRoomFacilityService roomFacilityService) : ControllerBase
    {
        private readonly IRoomFacilityService _roomFacilityService = roomFacilityService;

        [HttpGet("api/rooms/{roomId}/facilities")]
        public async Task<IActionResult> GetByRoomId(Guid roomId)
        {
            var result = await _roomFacilityService.GetByRoomIdAsync(roomId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("api/rooms/{roomId}/facilities")]
        public async Task<IActionResult> Create(Guid roomId, [FromBody] CreateRoomFacilityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomFacilityService.CreateAsync(roomId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("api/facilities/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomFacilityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomFacilityService.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("api/facilities/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roomFacilityService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
