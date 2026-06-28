using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TraineeManagement.Api.Messaging.RabbitMqConnection;

namespace TraineeManagement.Api.Extensions;

public static class HealthCheck
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddMySql(
                connectionString: configuration["ConnectionStrings:DefaultConnection"]!,
                name: "mysql",
                tags: ["readiness", "db"])
            .AddRedis(
                redisConnectionString: configuration["Redis:ConnectionString"]!,
                name: "redis",
                tags: ["readiness", "cache"])
            .AddRabbitMQ(
                sp =>
                {
                    RabbitConnection rabbitConn = sp.GetRequiredService<RabbitConnection>();
                    return rabbitConn.Connection!;
                },
                name: "rabbitmq",
                tags: ["readiness", "messaging"])
            .AddUrlGroup(
                uri: new Uri(configuration["DirectoryService:BaseUrl"] + "/api/health"),
                name: "directory-service",
                tags: ["readiness", "upstream"]);

        return services;
    }

    public static WebApplication MapAppHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false,
            ResponseWriter = WriteHealthResponse
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("readiness"),
            ResponseWriter = WriteHealthResponse
        });

        return app;
    }

    private static Task WriteHealthResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
        {
            Status = report.Status == HealthStatus.Healthy ? "ready" : "unavailable",
            Timestamp = DateTime.UtcNow
        }));
    }
}