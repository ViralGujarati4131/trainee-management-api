using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TraineeManagement.Api.Contract.SubmissionProcessingContarct;
using TraineeManagement.Api.Messaging.RabbitMqConnection;
using RabbitMQ.Client;

namespace TraineeManagement.Api.Messaging.RabbitMQPublisher;

public class RabbitMqService
{
    private readonly RabbitConnection _connection;

    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService(RabbitConnection connection, ILogger<RabbitMqService> logger)
    {
        _connection = connection;
        _logger = logger;
    }
    public async Task PublishAsync(SubmissionProcessingContract message)
    {
        if (_connection.Channel == null)
        {
            _logger.LogError("RabbitMQ channel is not initialized.");
            return;
        }

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };

        await _connection.Channel.BasicPublishAsync(
            exchange: "",
            routingKey: "submission-processing",
            mandatory: false,
            basicProperties: properties,
            body: body
        );

        _logger.LogInformation(
            "Published message: MessageId={MessageId}, CorrelationId={CorrelationId}, SubmissionId={SubmissionId}",
            message.MessageId, message.CorrelationId, message.SubmissionId
        );
    }
}
