using PropertyService.src.Application.DTOs.Requests.RoomTypeAmenity;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertyService.src.Web.Controllers
{
    [ApiController]
    [Route("api/room-types/{roomTypeId}/amenities")]
    [Authorize]
    public class RoomTypeAmenitiesController(IRoomTypeAmenityService roomTypeAmenityService) : ControllerBase
    {
        private readonly IRoomTypeAmenityService _roomTypeAmenityService = roomTypeAmenityService;

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid roomTypeId)
        {
            var result = await _roomTypeAmenityService.GetAmenitiesByRoomTypeIdAsync(roomTypeId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid roomTypeId, [FromBody] CreateRoomTypeAmenityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomTypeAmenityService.AddAmenityAsync(roomTypeId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{amenityId}")]
        public async Task<IActionResult> Remove(Guid roomTypeId, Guid amenityId)
        {
            var result = await _roomTypeAmenityService.RemoveAmenityAsync(roomTypeId, amenityId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
