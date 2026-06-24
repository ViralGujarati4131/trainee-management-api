using TraineeManagementApi.Users.DTOs;

namespace TraineeManagementApi.Users.ServiceInterface;

public interface IUserService
{
    Task<LoginTokenResponseDto> LoginUserAsync(UserLoginDto userLoginDto);
}