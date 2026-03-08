using NotificationService.src.Application.EventHandlers;

namespace NotificationService.src.Infrastructure.Messaging.Interfaces
{
    public interface IRabbitMqSubscriber
    {
        Task SubscribeToUserRegisteredEventsAsync(UserRegisteredEventHandler handler);
    }
}
