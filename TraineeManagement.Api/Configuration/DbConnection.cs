using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data.DatabaseContext;

namespace TraineeManagement.Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddAppDatabase(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 46));

        logger.LogInformation("Configuring database context. Version: {ServerVersion}", serverVersion);
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, serverVersion)
        );

        return services;
    }
}