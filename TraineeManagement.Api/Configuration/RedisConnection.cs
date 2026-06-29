using StackExchange.Redis;

namespace TraineeManagement.Api.Configuration;

public static class RedisConnection
{
    public static IServiceCollection AddRedisConnection(this IServiceCollection services, IConfiguration configuration, ILogger logger)
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