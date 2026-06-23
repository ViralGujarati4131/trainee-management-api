using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TraineeManagementApi.RabbitMQ.Settings;

public class RabbitMqService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName = string.Empty;

    public RabbitMqService(IOptions<RabbitMqSettings> options)
    {
        RabbitMqSettings settings = options.Value;
        _queueName = settings.QueueName;

        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = settings.Host,
            Port = settings.Port,
            VirtualHost = settings.VirtualHost,
            UserName = settings.Username,
            Password = settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public void Publish<T>(T message)
    {
        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        IBasicProperties properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(
            exchange: "",
            routingKey: _queueName,
            basicProperties: properties,
            body: body
        );
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}