namespace TenancyService.Application.DTOs.Requests.ContractFile
{
    public class AddContractFileRequest
    {
        public Guid ContractId { get; set; }
        public string FileId { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
    }
}
