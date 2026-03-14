using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenancyService.Application.DTOs.Requests.ContractFile;
using TenancyService.Application.Interfaces;

namespace TenancyService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractFilesController(IContractFileService contractFileService) : ControllerBase
    {
        private readonly IContractFileService _contractFileService = contractFileService;

        [HttpGet("contract/{contractId}")]
        public async Task<IActionResult> GetByContractId(Guid contractId)
        {
            var result = await _contractFileService.GetByContractIdAsync(contractId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddContractFileRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _contractFileService.AddAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            var result = await _contractFileService.RemoveAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
