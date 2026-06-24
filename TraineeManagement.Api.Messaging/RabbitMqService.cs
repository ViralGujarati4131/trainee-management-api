using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TraineeManagement.Api.Contract.Settings;

namespace TraineeManagement.Api.Messaging;

public class RabbitMqService : IDisposable
{
    private readonly RabbitMqSettings _settings;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    public RabbitMqService(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
    }

    // Helper method to safely initialize the connection and channel asynchronously
    private async Task EnsureChannelAsync()
    {
        if (_channel is { IsOpen: true }) return;

        await _connectionLock.WaitAsync();
        try
        {
            if (_channel is { IsOpen: true }) return;

            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            _connection ??= await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Declare queue asynchronously
            await _channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    // Changed to async to match RabbitMQ v7 Client signature
    public async Task PublishAsync<T>(T message)
    {
        await EnsureChannelAsync();

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        // v7 initialization pattern for persistent delivery properties
        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };

        // v7 uses BasicPublishAsync
        await _channel!.BasicPublishAsync(
            exchange: "",
            routingKey: _settings.QueueName,
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        _connectionLock.Dispose();
    }
}