using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.Application.DTOs.Requests.RoomPrice;
using PropertyService.Application.Interfaces;

namespace PropertyService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomPricesController(IRoomPriceService roomPriceService) : ControllerBase
    {
        private readonly IRoomPriceService _roomPriceService = roomPriceService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _roomPriceService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("room-type/{roomTypeId}/current")]
        public async Task<IActionResult> GetCurrentPrice(Guid roomTypeId)
        {
            var result = await _roomPriceService.GetCurrentPriceAsync(roomTypeId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("room-type/{roomTypeId}/history")]
        public async Task<IActionResult> GetHistory(Guid roomTypeId)
        {
            var result = await _roomPriceService.GetHistoryAsync(roomTypeId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomPriceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomPriceService.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomPriceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomPriceService.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roomPriceService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
