namespace PropertyService.Application.DTOs.Requests.Branch
{
    public class UpdateBranchRequest
    {
        public Guid ManagerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
