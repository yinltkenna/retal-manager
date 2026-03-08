using IdentityService.src.Application.DTOs.Requests.Authentication;
using IdentityService.src.Application.Services.Interfaces;
using IdentityService.src.Web.Common.TemplateResponses;

namespace IdentityService.src.Infrastructure.Clients
{
    public class TenancyServiceClient : ITenancyServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TenancyServiceClient> _logger;

        public TenancyServiceClient(
            HttpClient httpClient,
            ILogger<TenancyServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

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
