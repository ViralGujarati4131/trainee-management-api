using Microsoft.EntityFrameworkCore;
using PasswordHasher.Service.Interface;
using Users.Models;
using Users.Service.Interface;
using Users.DTOs;

// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using System.Security.Claims;

namespace Users.Service;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasherService _service;
    //  private readonly TokenService _tokenService;
    public UserService(AppDbContext context, IPasswordHasherService service)
    {
        _context = context;
        _service = service;
        // _tokenService = tokenService;
    }

    private LoginTokenResponseDto makeResponse(string t, int i, User u)
    {
        return new LoginTokenResponseDto
        {
            token = t,
            expiresIn = i,
            user = new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            }
        };
    }
    public async Task<LoginTokenResponseDto?> LoginUserAsync(UserLoginDto userLogin)
    {
        User? findUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userLogin.Username);
        if (findUser == null)
        {
            return null;
        }
        if (_service.VerifyPassword(userLogin.Password, findUser.PasswordHash))
        {
            // var token = _tokenService.GenerateToken(findUser.Id,findUser.Username,(findUser.Role).ToString());



            // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // var claims = new[]
            // {
            //     new Claim(ClaimTypes.Name, username),
            //     new Claim(ClaimTypes.Role, "Admin")
            // };
            // var token = new JwtSecurityToken(
            //     issuer: _config["Jwt:Issuer"],
            //     audience: _config["Jwt:Audience"],
            //     claims: claims,
            //     expires: DateTime.UtcNow.AddMinutes(60),
            //     signingCredentials: credentials);
            // return new JwtSecurityTokenHandler().WriteToken(token);




            var token = "JWT-Token";
            return makeResponse(token, 3600, findUser);
        }
        else
        {
            return null;
        }
    }
}