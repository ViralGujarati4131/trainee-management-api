using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TraineeManagement.Api.Data.DatabaseContext;
using TrainingDirectory.Api.DirectoryTraineeService;
using TrainingDirectory.Api.DirectoryTraineeServiceInterface;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.GlobalExceptionMiddleware;

var builder = WebApplication.CreateBuilder(args);

const string AllowedOriginsPolicy = "_myAllowSpecificOrigins";

// db connection
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 46));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);


string[] allowedOrigin = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
if (allowedOrigin.Length == 0)
{
    throw new ConfigurationMissingException(CustomResponse.ConfigurationMissingError);
}


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

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors(AllowedOriginsPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();
