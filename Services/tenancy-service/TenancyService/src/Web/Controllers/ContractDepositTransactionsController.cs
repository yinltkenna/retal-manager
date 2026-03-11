using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenancyService.src.Application.DTOs.Requests.ContractDepositTransaction;
using TenancyService.src.Application.Services.Interfaces;

namespace TenancyService.src.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractDepositTransactionsController(IContractDepositTransactionService contractDepositTransactionService) : ControllerBase
    {
        private readonly IContractDepositTransactionService _contractDepositTransactionService = contractDepositTransactionService;

        [HttpGet("contract/{contractId}")]
        public async Task<IActionResult> GetByContractId(Guid contractId)
        {
            var result = await _contractDepositTransactionService.GetByContractIdAsync(contractId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("contract/{contractId}")]
        public async Task<IActionResult> Create(Guid contractId, [FromBody] CreateContractDepositTransactionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _contractDepositTransactionService.CreateAsync(contractId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
