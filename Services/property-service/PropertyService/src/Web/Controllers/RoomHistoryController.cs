using PropertyService.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertyService.src.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class RoomHistoryController(IRoomHistoryService roomHistoryService) : ControllerBase
    {
        private readonly IRoomHistoryService _roomHistoryService = roomHistoryService;

        [HttpGet("api/rooms/{roomId}/history")]
        public async Task<IActionResult> GetByRoomId(Guid roomId)
        {
            var result = await _roomHistoryService.GetByRoomIdAsync(roomId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
