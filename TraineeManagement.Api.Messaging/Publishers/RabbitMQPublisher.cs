using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TraineeManagement.Api.Contract.SubmissionProcessingContarct;
using TraineeManagement.Api.Messaging.RabbitMqConnection;
using RabbitMQ.Client;
using TraineeManagement.Api.Data.Constants;

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
        IConnection connection = _connection.Connection!;

        if (connection == null || !connection.IsOpen)
        {
            _logger.LogError(
                "RabbitMQ publish failed. Reason={Reason}",
                "Connection unavailable");  
            return;
        }

        // To get the acknowledgement if succed to publish
        CreateChannelOptions channelOptions = new CreateChannelOptions(
            publisherConfirmationsEnabled: true,
            publisherConfirmationTrackingEnabled: true
        );

        await using IChannel channel = await connection.CreateChannelAsync(channelOptions);

        
        if (channel == null)
        {
            _logger.LogError(
                "RabbitMQ publish failed. Reason={Reason}",
                "Connection unavailable");
            return;
        }

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        BasicProperties properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
            // CorrelationId = message.CorrelationId,
            // MessageId = message.MessageId.ToString()
        };

        string targetExchange = AppConstants.RabbitMQ.GetExchange(AppConstants.RabbitMQ.SubmissionProcessing);
        string targetRoutingKey = AppConstants.RabbitMQ.GetRoutingKey(AppConstants.RabbitMQ.SubmissionProcessing);

        await channel.BasicPublishAsync(
            exchange: targetExchange,
            routingKey: targetRoutingKey,
            mandatory: true,
            basicProperties: properties,
            body: body
        );

        _logger.LogInformation(
            "RabbitMQ message published. MessageId={MessageId}, CorrelationId={CorrelationId}, SubmissionFileId={SubmissionFileId}",
            message.MessageId,
            message.CorrelationId,
            message.SubmissionFileId);
    }
}
