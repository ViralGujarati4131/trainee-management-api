// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Users.Config;

// namespace Users.Service
// {
// public class TokenService
// {
//     private readonly JwtSettings _jwtSettings;

//     public TokenService(JwtSettings jwtSettings)
//     {
//         _jwtSettings = jwtSettings;
//     }

//     public string GenerateToken(int userId, string username, string role)
//     {
//         var claims = new[]
//         {
//             new Claim("UserId", userId.ToString()),
//             new Claim("Username", username),
//             new Claim(ClaimTypes.Role, role)
//         };

//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
//         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//         var token = new JwtSecurityToken(
//             issuer: _jwtSettings.Issuer,
//             audience: _jwtSettings.Audience,
//             claims: claims,
//             expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresIn),
//             signingCredentials: creds
//         );

//         return new JwtSecurityTokenHandler().WriteToken(token);
//     }
// }
// }