using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.Application.DTOs.Requests.Reservation;
using PropertyService.Application.Interfaces;

namespace PropertyService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController(IReservationService reservationService) : ControllerBase
    {
        private readonly IReservationService _reservationService = reservationService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reservationService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reservationService.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _reservationService.CancelAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by-room/{roomId}")]
        public async Task<IActionResult> GetByRoom(Guid roomId)
        {
            var result = await _reservationService.GetByRoomIdAsync(roomId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by-tenant/{tenantId}")]
        public async Task<IActionResult> GetByTenant(Guid tenantId)
        {
            var result = await _reservationService.GetByTenantIdAsync(tenantId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
