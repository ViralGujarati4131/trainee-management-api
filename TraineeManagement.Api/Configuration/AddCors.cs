using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.Extensions;

public static class CorsExtensions
{
    public const string AllowedOriginsPolicy = "_myAllowSpecificOrigins";

    public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        string[] allowedOrigin = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        if (allowedOrigin.Length == 0)
        {
            logger.LogCritical("Dependency failure: CORS initialization parameters are missing.");
            throw new ConfigurationMissingException(CustomResponse.ConfigurationMissingError);
        }

        logger.LogInformation("Configuring CORS access profile layer.");
        services.AddCors(options =>
        {
            options.AddPolicy(name: AllowedOriginsPolicy, policy =>
            {
                policy.WithOrigins(allowedOrigin)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }
}