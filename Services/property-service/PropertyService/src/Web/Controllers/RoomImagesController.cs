using PropertyService.src.Application.DTOs.Requests.RoomImage;
using PropertyService.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertyService.src.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomImagesController(IRoomImageService roomImageService) : ControllerBase
    {
        private readonly IRoomImageService _roomImageService = roomImageService;

        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetByRoomId(Guid roomId)
        {
            var result = await _roomImageService.GetByRoomIdAsync(roomId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("room/{roomId}")]
        public async Task<IActionResult> Create(Guid roomId, [FromBody] CreateRoomImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomImageService.CreateAsync(roomId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/primary")]
        public async Task<IActionResult> SetPrimary(Guid id)
        {
            var result = await _roomImageService.SetPrimaryAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roomImageService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
