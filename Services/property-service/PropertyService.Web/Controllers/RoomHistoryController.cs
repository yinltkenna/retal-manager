using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.Application.Interfaces;

namespace PropertyService.Web.Controllers
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
