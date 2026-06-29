using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.Configuration;

public static class AuthJwtToken
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection jwtSettings = configuration.GetSection("JWT");
        string jwtKeyString = jwtSettings["Key"] ?? throw new ConfigurationMissingException(CustomResponse.ConfigurationMissingError);
        byte[] tokenSigningKey = Encoding.UTF8.GetBytes(jwtKeyString);

        services.AddAuthentication(options =>
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

        return services;
    }
}