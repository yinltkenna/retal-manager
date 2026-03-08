namespace IdentityService.src.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsActive { get; set; } = true; // For role Root System can hidden/ show permissions. Only Root System can edit type.
        public string Code { get; set; } // Unique code for the permission, e.g., "usser.create", "user.delete"
        public string Group { get; set; } // Grouping for permissions, e.g., "Order", "Role"
        public string Name { get; set; } // Display name for the permission, e.g., "Create User", "Delete Role"
        public string Type { get; set; } // e.g., "Menu", "Button", "API"
        public string? UrlIcon { get; set; } // Icon URL or class name for UI representation
        public string? Description { get; set; }
        public int SortOrder { get; set; } // For ordering permissions in UI
    }
}
