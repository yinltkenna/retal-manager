using IdentityService.src.Web.Configurations;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace IdentityService.src.Infrastructure.Messaging.Publishers
{
    public class RabbitMqPublisher : IRabbitMqPublisher, IAsyncDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly ConnectionFactory _factory;

        public RabbitMqPublisher(RabbitMqSettings settings)
        {
            _factory = new ConnectionFactory
            {
                HostName = settings.Host,
                UserName = settings.Username,
                Password = settings.Password
            };
        }

        private async Task InitializeAsync()
        {
            if (_connection == null)
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }
        }

        public async Task PublishAsync<T>(T message, string exchange, string routingKey)
        {
            await InitializeAsync();

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await _channel!.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Topic, durable: true);

            await _channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                body: body);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null) await _channel.DisposeAsync();
            if (_connection != null) await _connection.DisposeAsync();
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}