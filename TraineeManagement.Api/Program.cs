using TraineeManagement.Api.Configuration;
using TraineeManagement.Api.GlobalExceptionMiddleware;
using Newtonsoft.Json;
using TraineeManagement.Api.CorrelationIdMiddleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

using ILoggerFactory loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
ILogger logger = loggerFactory.CreateLogger("Program");

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
    .AddNewtonsoftJson(o => o.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Error);

builder.AddSerilogLogging(); 
builder.Services.AddMySqlConnection(builder.Configuration, logger);
builder.Services.AddRedisConnection(builder.Configuration, logger);
builder.Services.AddRabbitMqConnection(builder.Configuration);
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration, logger);
builder.Services.AddHttpClient(builder.Configuration, logger);
builder.Services.AddFileStoreConfig(builder.Configuration);
builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddDependencyInjection();


WebApplication app = builder.Build();

await app.InitializeMessagingAsync(logger);
await UserSeeder.SeedAsync(app.Services);

app.UseMiddleware<CorrelationIdMiddleware>();  
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors(SetFrontendCors.AllowedOriginsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapAppHealthChecks();

app.Run();