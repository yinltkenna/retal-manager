namespace IdentityService.Application.DTOs.Requests.Permession
{
    public class UpdatePermissionRequest
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public bool? IsActive { get; set; } = true; // For role Root System can hidden/ show permissions. Only Root System can edit type.
        public string? Code { get; set; } = string.Empty; // Unique code for the permission, e.g., "create", "delete"
        public string? Group { get; set; } = string.Empty; // Grouping for permissions, e.g., "Order", "Role"
        public string? Name { get; set; } = string.Empty; // Display name for the permission, e.g., "Create User", "Delete Role"
        public string? Type { get; set; } = string.Empty; // e.g., "Menu", "Button", "API"
        public string? UrlIcon { get; set; } = string.Empty; // Icon URL or class name for UI representation
        public string? Description { get; set; } = string.Empty;
        public int? SortOrder { get; set; } // For ordering permissions in UI
    }
}
