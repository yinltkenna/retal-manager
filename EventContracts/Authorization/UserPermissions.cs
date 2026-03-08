namespace EventContracts.Authorization
{
    public static class UserPermissions
    {
        // Basic
        public const string VIEW = "user.view";
        public const string CREATE = "user.create";
        public const string UPDATE = "user.update";
        public const string DELETE = "user.delete";

        // Security
        public const string LOCK = "user.lock";

        // Logs
        public const string VIEW_LOGS = "user.view_logs";
    }
}
