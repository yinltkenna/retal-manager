using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.Application.Interfaces;

namespace PropertyService.Web.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    [Authorize]
    public class RoomAvailabilityController(IReservationService reservationService) : ControllerBase
    {
        private readonly IReservationService _reservationService = reservationService;

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reservationService.GetAvailableRoomsAsync(startDate, endDate);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
