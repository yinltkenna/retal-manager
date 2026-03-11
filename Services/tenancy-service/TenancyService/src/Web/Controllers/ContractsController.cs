using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenancyService.src.Application.DTOs.Requests.Contract;
using TenancyService.src.Application.Services.Interfaces;

namespace TenancyService.src.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractsController(IContractService contractService) : ControllerBase
    {
        private readonly IContractService _contractService = contractService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contractService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _contractService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContractRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _contractService.CreateAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContractRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _contractService.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _contractService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
