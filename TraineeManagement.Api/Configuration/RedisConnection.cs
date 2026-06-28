using StackExchange.Redis;

namespace TraineeManagement.Api.Extensions;

public static class RedisExtensions
{
    public static IServiceCollection AddAppRedis(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        logger.LogInformation("Registering distributed cache engine client dependencies.");
        
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            string connectionString = configuration["Redis:ConnectionString"]!;
            return ConnectionMultiplexer.Connect(connectionString);
        });

        return services;
    }
}