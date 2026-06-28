using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Messaging.RabbitMqConnectionSettings;
using TraineeManagement.Api.Worker.SubmissionProcessingConsumer;   
using TraineeManagement.Api.Data.DatabaseContext;
using TraineeManagement.Api.Messaging.RabbitMqConnection;
using TraineeManagement.Api.Data.CacheService;
using TraineeManagement.Api.Data.CacheServiceInterface;
using StackExchange.Redis;
using TraineeManagement.Api.Data.FileStoreValidation;
using TraineeManagement.Api.Data.Constants;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Setup structural bootstrapper logger
using ILoggerFactory loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
ILogger logger = loggerFactory.CreateLogger("Program");

logger.LogInformation("Initializing worker host application setup.");

// mysql db connection
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 46));

logger.LogInformation("Configuring database context. Version: {ServerVersion}", serverVersion);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);

// get rabbitmq credentials
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));

// dependancy Injections
builder.Services.AddSingleton<ICacheService, CacheService>();

// redis connection
logger.LogInformation("Registering cache dependency multiplexer service.");
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    string configuration = builder.Configuration["Redis:ConnectionString"]!;
    return ConnectionMultiplexer.Connect(configuration);
});

// get file store configurations
builder.Services.Configure<CustomFileStoreValidation>(
    builder.Configuration.GetSection(AppConstants.ConfigSections.FileStorage)
);

// rabbit connection
builder.Services.AddSingleton<RabbitConnection>();
builder.Services.AddHostedService<SubmissionProcessingConsumer>();


IHost host = builder.Build();

logger.LogInformation("State transition: Building messaging infrastructure client pipelines.");

RabbitConnection conn = host.Services.GetRequiredService<RabbitConnection>();
try
{
    await conn.InitializeAsync();
    logger.LogInformation("Publish/consume connection initialized successfully.");
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Dependency failure: Broker initialization aborted during application startup.");
    throw;
}

logger.LogInformation("State transition: Starting host worker runtime execution loop.");
await host.RunAsync();