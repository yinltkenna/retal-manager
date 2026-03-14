namespace IdentityService.Application.Configurations
{
    public class RedisSettings
    {
        public string Connection { get; set; } = string.Empty;
        public string? Password { get; set; }
        public int Database { get; set; } = 0;
    }
}