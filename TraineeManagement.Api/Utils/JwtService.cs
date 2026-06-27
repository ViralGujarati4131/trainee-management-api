using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.Data.UserModel;
using TraineeManagement.Api.Data.Constants;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.ResponsesBuilder;

namespace TraineeManagement.Api.JwtService;

public interface IJwtService
{
    string GenerateJwtToken(User user, out int expiryMinutes);
}

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string GenerateJwtToken(User user, out int expiryMinutes)
    {
        _logger.LogInformation("Retrieving configuration settings and claims to generate JWT token for UserId: {UserId}", user.Id);

        IConfigurationSection jwtSettings = _configuration.GetSection("Jwt");
        
        string secretKey = jwtSettings["Key"] ?? throw new ConfigurationMissingException(CustomResponse.ConfigurationMissingError);

        if (!int.TryParse(jwtSettings["ExpiryMinutes"], out expiryMinutes))
        {
            expiryMinutes = AppConstants.Security.DefaultExpiryMinutes;
        }

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims = new[]
        {
            new Claim(AppConstants.Security.ClaimId, user.Id.ToString()),
            new Claim(AppConstants.Security.ClaimUsername, user.Username),
            new Claim(AppConstants.Security.ClaimRole, user.Role.ToString() ?? AppConstants.Security.DefaultRole)
        };

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = credentials
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        string generatedToken = tokenHandler.WriteToken(token);
        
        if (string.IsNullOrWhiteSpace(generatedToken))
        {
            throw new JwtOperationException(CustomResponse.JwtOperationError);
        }

        _logger.LogInformation("JWT token successfully generated for UserId: {UserId}", user.Id);
        return generatedToken;
    }
}