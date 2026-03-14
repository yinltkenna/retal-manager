namespace PropertyService.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public Guid ManagerId { get; set; } // From Identity-Service
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
