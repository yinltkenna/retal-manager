using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using EventContracts.Identity;
using NotificationService.src.Application.EventHandlers;
using NotificationService.src.Web.Configurations;
using NotificationService.src.Infrastructure.Messaging.Interfaces;

namespace NotificationService.src.Infrastructure.Messaging
{
    public class RabbitMqSubscriber : IRabbitMqSubscriber, IAsyncDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly ConnectionFactory _factory;
        private readonly ILogger<RabbitMqSubscriber> _logger;

        public RabbitMqSubscriber(
            RabbitMqSettings settings,
            ILogger<RabbitMqSubscriber> logger)
        {
            _logger = logger;

            _factory = new ConnectionFactory()
            {
                HostName = settings.Host,
                UserName = settings.Username,
                Password = settings.Password
            };
        }

        private async Task EnsureConnectedAsync()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }
        }

        public async Task SubscribeToUserRegisteredEventsAsync(UserRegisteredEventHandler handler)
        {
            try
            {
                await EnsureConnectedAsync();

                if (_channel == null)
                    throw new InvalidOperationException("Channel is not initialized");

                // Declare exchange
                await _channel.ExchangeDeclareAsync(
                    exchange: "identity.exchange",
                    type: ExchangeType.Topic,
                    durable: true);

                // Declare queue
                var queueName = "notification.user-registered";
                await _channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Bind queue to exchange
                await _channel.QueueBindAsync(
                    queue: queueName,
                    exchange: "identity.exchange",
                    routingKey: "user.registered");

                _logger.LogInformation("✅ Subscribed to 'user.registered' events");

                // Start consuming messages
                _ = Task.Run(async () => await ConsumeMessagesAsync(queueName, handler), CancellationToken.None);

                _logger.LogInformation("🚀 RabbitMQ consumer started");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to RabbitMQ events");
                throw;
            }
        }

        private async Task ConsumeMessagesAsync(string queueName, UserRegisteredEventHandler handler)
        {
            try
            {
                if (_channel == null)
                    throw new InvalidOperationException("Channel is null");

                // Set QoS to process one message at a time
                await _channel.BasicQosAsync(0, 1, false);

                // Get first message
                var result = await _channel.BasicGetAsync(queueName, false);
                
                while (result != null)
                {
                    try
                    {
                        var body = result.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        
                        _logger.LogInformation("📦 Received message from queue: {Message}", message);

                        // Deserialize event
                        var @event = JsonConvert.DeserializeObject<UserRegisteredEvent>(message);
                        if (@event != null)
                        {
                            // Handle event
                            await handler.HandleAsync(@event);
                        }

                        // Acknowledge message
                        await _channel.BasicAckAsync(result.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message");
                        // Nack message to requeue
                        if (result != null)
                        {
                            await _channel.BasicNackAsync(result.DeliveryTag, false, true);
                        }
                    }

                    // Get next message - with small delay to prevent busy waiting
                    await Task.Delay(100);
                    result = await _channel.BasicGetAsync(queueName, false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer error in ConsumeMessagesAsync");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null) await _channel.DisposeAsync();
            if (_connection != null) await _connection.DisposeAsync();
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().Wait();
        }
    }
}
