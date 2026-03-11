namespace TenancyService.src.Web.Configurations
{
    public class RedisSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string? Password { get; set; }
    }
}
