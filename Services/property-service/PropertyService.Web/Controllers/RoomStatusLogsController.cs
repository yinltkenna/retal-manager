using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.Application.Interfaces;

namespace PropertyService.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class RoomStatusLogsController(IRoomStatusLogService statusLogService) : ControllerBase
    {
        private readonly IRoomStatusLogService _statusLogService = statusLogService;

        [HttpGet("api/rooms/{roomId}/status-logs")]
        public async Task<IActionResult> GetByRoomId(Guid roomId)
        {
            var result = await _statusLogService.GetByRoomIdAsync(roomId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
