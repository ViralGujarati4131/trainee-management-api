using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data.DatabaseContext;

namespace TraineeManagement.Api.Configuration;

public static class MySqlConnection
{
    public static IServiceCollection AddMySqlConnection(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 46));

        logger.LogInformation("Configuring database context.");
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, serverVersion)
        );

        return services;
    }
}