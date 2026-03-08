namespace PropertyService.src.Domain.Entities
{
    public class Brand
    {
        public Guid Id { get; set; }
        public Guid ManagerId { get; set; } // From Identity-Service
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
