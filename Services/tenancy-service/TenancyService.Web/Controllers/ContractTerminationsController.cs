using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenancyService.Application.DTOs.Requests.ContractTermination;
using TenancyService.Application.Interfaces;

namespace TenancyService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractTerminationsController(IContractTerminationService contractTerminationService) : ControllerBase
    {
        private readonly IContractTerminationService _contractTerminationService = contractTerminationService;

        [HttpGet("contract/{contractId}")]
        public async Task<IActionResult> GetByContractId(Guid contractId)
        {
            var result = await _contractTerminationService.GetByContractIdAsync(contractId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("contract/{contractId}")]
        public async Task<IActionResult> Create(Guid contractId, [FromBody] CreateContractTerminationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _contractTerminationService.CreateAsync(contractId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
