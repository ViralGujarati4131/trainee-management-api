using Users.DTOs;

namespace Users.Service.Interface;

public interface IUserService
{
    public Task<LoginTokenResponseDto?> LoginUserAsync(UserLoginDto userLogin);
}