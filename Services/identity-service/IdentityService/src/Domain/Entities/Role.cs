namespace IdentityService.src.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = false; // To show or hide role in UI.
        public bool IsDeleted { get; set; } = false;
    }
}
