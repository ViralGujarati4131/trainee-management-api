using TraineeManagement.Api.Extensions;
using TraineeManagement.Api.GlobalExceptionMiddleware;
using Newtonsoft.Json;
using TraineeManagement.Api.CorrelationIdMiddleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

using ILoggerFactory loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
ILogger logger = loggerFactory.CreateLogger("Program");

logger.LogInformation("Initializing web API pipeline container host.");

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
    .AddNewtonsoftJson(o => o.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Error);

builder.Services.AddAppDatabase(builder.Configuration, logger);
builder.Services.AddAppRedis(builder.Configuration, logger);
builder.Services.AddAppMessaging(builder.Configuration);
builder.Services.AddAppAuth(builder.Configuration);
builder.Services.AddAppCors(builder.Configuration, logger);
builder.Services.AddAppHttpClients(builder.Configuration, logger);
builder.Services.AddAppFileStorage(builder.Configuration);
builder.Services.AddAppHealthChecks(builder.Configuration);
builder.Services.AddApplicationServices();

WebApplication app = builder.Build();

await app.InitializeMessagingAsync(logger);
await UserSeeder.SeedAsync(app.Services);

app.UseMiddleware<CorrelationIdMiddleware>();  
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors(CorsExtensions.AllowedOriginsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapAppHealthChecks();

logger.LogInformation("State transition: Launching host thread run application loop.");
app.Run();