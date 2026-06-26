using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Messaging.RabbitMqConnectionSettings;
using TraineeManagement.Api.Worker.SubmissionProcessingConsumer;   
using TraineeManagement.Api.Data.DatabaseContext;
using TraineeManagement.Api.Messaging.RabbitMqConnection;
using TraineeManagement.Api.CacheService;
using TraineeManagement.Api.CacheServiceInterface;
using StackExchange.Redis;
using TraineeManagement.Api.FileStoreValidation;
using TraineeManagement.Api.Data.Constants;
using Polly;
using System.Net.Http.Headers;
using TraineeManagement.Api.Worker.Services;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);




[cite_start]// Register the client with standard resilience controls [cite: 413, 422]
builder.Services.AddHttpClient<TrainingDirectoryClient>(client =>
{
    [cite_start]client.BaseAddress = new Uri("http://localhost:5005/"); // Base Uri of your new service [cite: 413]
    client.Timeout = TimeSpan.FromSeconds(5); // 1. Finite timeout configuration [cite: 420]
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddStandardResilienceHandler(options =>
{
    [cite_start]// 2. Linear/Exponential retry logic configuration for transient failures [cite: 421]
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromSeconds(1);
    options.Retry.BackoffType = DelayBackoffType.Constant;

    [cite_start]// 3. Circuit breaker options to prevent system cascading failures [cite: 422]
    options.CircuitBreaker.FailureRatio = 0.5; // Trip circuit if 50% of requests fail [cite: 422]
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(10);
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15);
});













string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 46));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSingleton<ICacheService, CacheService>();

// redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    string configuration = builder.Configuration["Redis:ConnectionString"]!;
    return ConnectionMultiplexer.Connect(configuration);
});

// rabbit connection
builder.Services.AddSingleton<RabbitConnection>();
builder.Services.AddHostedService<SubmissionProcessingConsumer>();


builder.Services.Configure<CustomFileStoreValidation>(
    builder.Configuration.GetSection(AppConstants.ConfigSections.FileStorage)
);

IHost host = builder.Build();

using (IServiceScope scope = host.Services.CreateScope())
{
    RabbitConnection conn = scope.ServiceProvider.GetRequiredService<RabbitConnection>();
    await conn.InitializeAsync();
}

await host.RunAsync();