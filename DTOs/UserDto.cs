using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Users.Models;

namespace TraineeManagementApi.Users.DTOs;

public record UserResponseDto
(
     int Id,
     string Username,
     UserRole Role
);

public record LoginTokenResponseDto
(
    string Token,
    int ExpiresIn,
    UserResponseDto? User
);

public record UserLoginDto
(
    [Required(ErrorMessage = "Username can not be empty")]
    string Username,

    [Required(ErrorMessage = "Password can not be empty")]
    string Password
);