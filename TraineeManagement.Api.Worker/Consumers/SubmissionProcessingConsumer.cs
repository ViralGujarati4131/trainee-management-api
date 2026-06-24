using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TraineeManagement.Api.Contract.SubmissionProcessingContarct;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data.AppDbContext;
using TraineeManagement.Api.Data.TaskAssignmentModel;
using TraineeManagement.Api.Messaging.RabbitMqConnection;
using TraineeManagement.Api.Data.Constants;

namespace TraineeManagement.Api.Worker.SubmissionProcessingConsumer;

public class SubmissionProcessingConsumer : BackgroundService
{
    private readonly ILogger<SubmissionProcessingConsumer> _logger;
    private readonly RabbitConnection _connection;  
    private readonly IServiceScopeFactory _scopeFactory;

    public SubmissionProcessingConsumer(ILogger<SubmissionProcessingConsumer> logger, RabbitConnection connection,IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _connection = connection;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_connection.Channel is null)
        {
            _logger.LogError("RabbitMQ channel is not initialized.");
        return;
        }
        var consumer = new AsyncEventingBasicConsumer(_connection.Channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            SubmissionProcessingContract? message = null;
            try
            {
                message = JsonSerializer.Deserialize<SubmissionProcessingContract>(json);

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

                await _connection.Channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);

                _logger.LogInformation("Successfully processed SubmissionId={SubmissionId}", message.SubmissionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message: {Json}", json);

                await _connection.Channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
            }
        };

        await _connection.Channel.BasicConsumeAsync(
            queue: AppConstants.RabbitMQ.SubmissionProcessing,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection?.Channel is not null)
            await _connection.Channel.DisposeAsync();

        if (_connection is not null)
            await _connection.DisposeAsync();

        await base.StopAsync(cancellationToken);
    }
}