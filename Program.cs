using TraineeManagementApi.Service.Interface;
using TraineeManagementApi.Service;
using Microsoft.EntityFrameworkCore;
using PasswordHasher.Service.Interface;
using PasswordHasher.Service;
using Users.Service.Interface;
using Users.Service;
// using Users.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters
.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 46));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);

builder.Services.AddScoped<ITraineeService, TraineeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();




// builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
// var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

// builder.Services.AddScoped<TokenService>();

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = jwtSettings.Issuer,
//         ValidAudience = jwtSettings.Audience,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
//     };
// });


// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddJwtBearer(options =>
// {
// options.TokenValidationParameters = new TokenValidationParameters
// {
// ValidateIssuer = true,
// ValidateAudience = true,
// ValidateLifetime = true,
// ValidateIssuerSigningKey = true,
// ValidIssuer = builder.Configuration["Jwt:TraineeManagementApi"],
// ValidAudience = builder.Configuration["Jwt:TraineeManagementClient"],
// IssuerSigningKey = new SymmetricSecurityKey(
// Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Strong_Key"]))
// };
// });

// builder.Services.AddAuthorization();





builder.Services.AddOpenApi();
var app = builder.Build();

await UserSeeder.SeedAsync(app.Services, new PasswordHasherService());

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
