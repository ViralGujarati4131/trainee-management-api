using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TraineeManagementApi.GlobalExceptionMiddleware;
using TraineeManagementApi.Utils.UserSeeder;
// using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using TraineeManagementApi.FileStorage.Configurations;
using TraineeManagementApi.Constants;
using StackExchange.Redis;
using System.Data;
using MySqlConnector;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// for react cors
const string AllowedOriginsPolicy = "_myAllowSpecificOrigins";

// structured logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// to allow the ModelState Validation instead of [ApiController]
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// to do not allow any extra field with new name and value
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
       options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Error; 
    });


// db connection
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 46));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);


// redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    string configuration = builder.Configuration["Redis:ConnectionString"]!;
    return ConnectionMultiplexer.Connect(configuration);
});


// // to take bearer token from user
// builder.Services.AddOpenApi("v1", options =>
// {
//     options.AddDocumentTransformer((document, context, cancellationToken) =>
//     {
//         OpenApiSecurityScheme scheme = new OpenApiSecurityScheme
//         {
//             Type = SecuritySchemeType.Http,
//             Scheme = "bearer",
//             BearerFormat = "JWT",
//             In = ParameterLocation.Header,
//             Description = "Enter your JWT token directly"
//         };
//         document.Components ??= new OpenApiComponents();
//         document.Components.SecuritySchemes.Add("Bearer", scheme);
//         document.SecurityRequirements.Add(new OpenApiSecurityRequirement
//         {
//             [new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             }] = Array.Empty<string>()
//         });
//         return Task.CompletedTask;
//     });
// });

// to validate the bearer token
IConfigurationSection jwtSettings = builder.Configuration.GetSection("JWT");
string jwtKeyString = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing from configuration.");
byte[] tokenSigningKey = Encoding.UTF8.GetBytes(jwtKeyString);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(tokenSigningKey)
    };
});

builder.Services.Configure<FileStorageConfiguration>(
    builder.Configuration.GetSection(AppConstants.ConfigSections.FileStorage)
);

// Add your infrastructure/application services cleanly
builder.Services.AddApplicationServices();

// react origin
string[] allowedOrigin = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
if (allowedOrigin.Length == 0)
{
    throw new InvalidOperationException(
        $"CORS configuration missing for environment: {builder.Environment.EnvironmentName}"
    );
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

// builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

// seed the user
await UserSeeder.SeedAsync(app.Services);

// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
//     app.UseSwaggerUi(options =>
//     {
//         options.DocumentPath = "/openapi/v1.json";
//     });
// }

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors(AllowedOriginsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();