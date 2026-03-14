using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenancyService.Application.DTOs.Requests.ContractExtension;
using TenancyService.Application.Interfaces;

namespace TenancyService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractExtensionsController(IContractExtensionService contractExtensionService) : ControllerBase
    {
        private readonly IContractExtensionService _contractExtensionService = contractExtensionService;

        [HttpGet("contract/{contractId}")]
        public async Task<IActionResult> GetByContractId(Guid contractId)
        {
            var result = await _contractExtensionService.GetByContractIdAsync(contractId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("contract/{contractId}")]
        public async Task<IActionResult> Create(Guid contractId, [FromBody] CreateContractExtensionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _contractExtensionService.CreateAsync(contractId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
