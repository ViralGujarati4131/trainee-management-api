using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TraineeManagement.Api.Data.DatabaseContext;
using TrainingDirectory.Api.DirectoryTraineeService;
using TrainingDirectory.Api.DirectoryTraineeServiceInterface;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.GlobalExceptionMiddleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Setup structural bootstrapper logger
using ILoggerFactory loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
ILogger logger = loggerFactory.CreateLogger("Program");

logger.LogInformation("Initializing application setup.");

const string AllowedOriginsPolicy = "_myAllowSpecificOrigins";

// db connection
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 46));

logger.LogInformation("Configuring database context. Version: {ServerVersion}", serverVersion);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);

string[] allowedOrigin = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
if (allowedOrigin.Length == 0)
{
    logger.LogCritical("Dependency failure: CORS configuration is missing.");
    throw new ConfigurationMissingException(CustomResponse.ConfigurationMissingError);
}

logger.LogInformation("Configuring CORS policy. AllowedOriginsCount: {Count}", allowedOrigin.Length);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOriginsPolicy,
                      policy =>
                      {
                          policy.WithOrigins(allowedOrigin)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});

builder.Services.AddScoped<IDirectoryTraineeService,DirectoryTraineeService>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

WebApplication app = builder.Build();

logger.LogInformation("State transition: Building application pipeline.");

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors(AllowedOriginsPolicy);
app.UseAuthorization();
app.MapControllers();

logger.LogInformation("State transition: Starting host application.");
app.Run();