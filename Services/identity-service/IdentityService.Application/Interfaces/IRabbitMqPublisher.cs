namespace IdentityService.Application.Interfaces
{
    public interface IRabbitMqPublisher
    {
        Task PublishAsync<T>(T message, string exchange, string routingKey);
    }
}