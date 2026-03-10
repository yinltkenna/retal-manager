namespace EventContracts.Authorization.Permissions.IdentityService
{
    public static class UserPermissions
    {
        // Basic
        public const string VIEW = "identity.user.view";
        public const string CREATE = "identity.user.create";
        public const string UPDATE = "identity.user.update";
        public const string DELETE = "identity.user.delete";

        // Security
        public const string LOCK = "identity.user.lock";

        // Logs
        public const string VIEW_LOGS = "identity.user.view_logs";
    }
}
