using TraineeManagement.Api.Data.UserModel;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.UserDTO;

public record UserResponseDto
(
    int Id,

    string Username,

    UserRole? Role
);

public record LoginTokenResponseDto
(
    string Token,

    int ExpiresIn,
    
    UserResponseDto? User
);

public record UserLoginDto
(
    [RequiredField]
    string Username,

    [RequiredField]
    string Password
);