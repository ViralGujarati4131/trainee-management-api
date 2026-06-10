using System.ComponentModel.DataAnnotations;
using Users.Models;

namespace Users.DTOs;

public class UserResponseDto
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Username can not be empty")]
    public string Username { get; set; } = string.Empty;

    [AllowedValues(UserRole.Admin, UserRole.Mentor, UserRole.Trainee)]
    public UserRole Role { get; set; }
}

public class LoginTokenResponseDto
{
    public string token { get; set; } = string.Empty;
    public int expiresIn { get; set; }
    public UserResponseDto? user { get; set; }
}

public class UserLoginDto
{
    [Required(ErrorMessage = "Username can not be empty")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password can not be empty")]
    public string Password { get; set; } = string.Empty;

}