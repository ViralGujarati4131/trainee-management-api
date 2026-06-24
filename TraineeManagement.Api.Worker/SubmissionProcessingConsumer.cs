using Microsoft.Extensions.DependencyInjection; // Required for IServiceScopeFactory
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TraineeManagement.Api.Contract.Settings;
using TraineeManagementApi.Messaging.Contracts;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagementApi.TaskAssignments.Models;

namespace TraineeManagement.Api.Worker.Consumers;

public class SubmissionProcessingConsumer : BackgroundService
{
    private readonly ILogger<SubmissionProcessingConsumer> _logger;
    private readonly RabbitMqSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory; 
    private IConnection? _connection;
    private IChannel? _channel;

    public SubmissionProcessingConsumer(
        ILogger<SubmissionProcessingConsumer> logger, 
        IOptions<RabbitMqSettings> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _settings = options.Value;
        _scopeFactory = scopeFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker attempting connection using appsettings configuration...");

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            VirtualHost = _settings.VirtualHost,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
        );

        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: cancellationToken);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            SubmissionProcessingRequested? message = null;
            try
            {
                message = JsonSerializer.Deserialize<SubmissionProcessingRequested>(json);

                if (message == null)
                    throw new Exception("Invalid message format.");

                _logger.LogInformation(
                    "Processing SubmissionId={SubmissionId}, MessageId={MessageId}, CorrelationId={CorrelationId}",
                    message.SubmissionId, message.MessageId, message.CorrelationId
                );

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var submission = await dbContext.Submissions
                        .FirstOrDefaultAsync(s => s.Id == message.SubmissionId, stoppingToken);

                    if (submission == null)
                    {
                        _logger.LogWarning("Submission with ID {SubmissionId} not found in database.", message.SubmissionId);
                    }
                    else
                    {
                        var assignmentId = submission.TaskAssignmentId;

                        var assignment = await dbContext.TaskAssignments
                            .FirstOrDefaultAsync(a => a.Id == assignmentId, stoppingToken);

                        if (assignment != null)
                        {
                            assignment.Status = TaskAssignmentStatus.Submitted; 

                            await dbContext.SaveChangesAsync(stoppingToken);
                            
                            _logger.LogInformation("Successfully updated TaskAssignmentId={AssignmentId} Status to 2", assignmentId);
                        }
                        else
                        {
                            _logger.LogWarning("TaskAssignment with ID {AssignmentId} not found.", assignmentId);
                        }
                    }
                }

                await _channel!.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);

                _logger.LogInformation("Successfully processed SubmissionId={SubmissionId}", message.SubmissionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message: {Json}", json);

                await _channel!.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
            }
        };

        await _channel!.BasicConsumeAsync(
            queue: _settings.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}