using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.UserDTO;
using TraineeManagement.Api.UserServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.UserController;

[ApiController]
[Route(CustomConstRoute.Auth)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost(CustomConstRoute.Login)]
    public async Task<ActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        _logger.LogInformation("Login attempt initiated for Username: {Username}", userLoginDto.Username);

        LoginTokenResponseDto authenticationResult = await _userService.LoginUserAsync(userLoginDto);
        
        _logger.LogInformation("Authentication successful. Session token issued for Username: {Username}", userLoginDto.Username);
        
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.LoginSuccess,
            authenticationResult
        );
    }
}