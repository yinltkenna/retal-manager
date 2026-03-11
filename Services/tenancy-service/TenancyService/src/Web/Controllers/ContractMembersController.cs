using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenancyService.src.Application.DTOs.Requests.ContractMember;
using TenancyService.src.Application.Services.Interfaces;

namespace TenancyService.src.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractMembersController(IContractMemberService contractMemberService) : ControllerBase
    {
        private readonly IContractMemberService _contractMemberService = contractMemberService;

        [HttpGet("contract/{contractId}")]
        public async Task<IActionResult> GetByContractId(Guid contractId)
        {
            var result = await _contractMemberService.GetByContractIdAsync(contractId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddContractMemberRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _contractMemberService.AddAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            var result = await _contractMemberService.RemoveAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
