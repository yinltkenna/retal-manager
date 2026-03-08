namespace IdentityService.src.Infrastructure.Messaging.Publishers
{
    public interface IRabbitMqPublisher
    {
        Task PublishAsync<T>(T message, string exchange, string routingKey);
    }
}