using TraineeManagement.Api.Messaging.RabbitMQPublisher;
using TraineeManagement.Api.Messaging.RabbitMqConnection;
using TraineeManagement.Api.Messaging.RabbitMqConnectionSettings;

namespace TraineeManagement.Api.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddAppMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<RabbitConnection>();
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
        services.AddSingleton<RabbitMqService>();

        return services;
    }

    public static async Task InitializeMessagingAsync(this WebApplication app, ILogger logger)
    {
        using IServiceScope scope = app.Services.CreateScope();
        RabbitConnection conn = scope.ServiceProvider.GetRequiredService<RabbitConnection>();
        try
        {
            await conn.InitializeAsync();
            logger.LogInformation("Publish/consume connection initialized successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Dependency failure: Messaging initialization collapsed during application startup.");
            throw;
        }
    }
}