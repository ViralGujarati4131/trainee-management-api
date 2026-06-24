using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Contract.Settings;
using TraineeManagement.Api.Worker.Consumers;   
using TraineeManagement.Api.Data; 

var builder = Host.CreateApplicationBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 46));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.AddHostedService<SubmissionProcessingConsumer>();

var host = builder.Build();
await host.RunAsync();