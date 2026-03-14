using IdentityService.Application.Common.TemplateResponses;
using IdentityService.Application.DTOs.Requests.Authentication;
using IdentityService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace IdentityService.Infrastructure.Clients
{
    public class TenancyServiceClient(
        HttpClient httpClient,
        ILogger<TenancyServiceClient> logger) : ITenancyServiceClient
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<TenancyServiceClient> _logger = logger;

        public async Task<Guid?> ClaimContractAsync(string codeContract, Guid userId)
        {
            try
            {
                var request = new ClaimContractRequest
                {
                    CodeContract = codeContract,
                    UserId = userId
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "/api/contracts/claim",
                    request
                );

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "ClaimContract failed with status {StatusCode}",
                        response.StatusCode
                    );
                    return null;
                }

                var result = await response.Content
                    .ReadFromJsonAsync<ApiResponse<Guid>>();

                if (result == null || !result.Success)
                    return null;

                return result.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling TenancyService ClaimContract");
                return null;
            }
        }
    }
}
