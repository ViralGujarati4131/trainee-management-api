using TraineeManagement.Api.Data.UserDTO;

namespace TraineeManagement.Api.UserServiceInterface;

public interface IUserService
{
    Task<LoginTokenResponseDto> LoginUserAsync(UserLoginDto userLoginDto);
}