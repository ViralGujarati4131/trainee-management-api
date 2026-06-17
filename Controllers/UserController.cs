using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Users.DTOs;
using TraineeManagementApi.Users.ServiceInterface;
using TraineeManagementApi.ResponsesBuilder;

namespace TraineeManagementApi.Users.Controller;

[ApiController]
[Route("api/auth")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginTokenResponseDto>> LoginUser([FromBody] UserLoginDto userLoginDto)
    {
        if(!ModelState.IsValid)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest,"Validation failed",ModelState);
        }
        _logger.LogInformation("Login attempt initiated for Username: {Username}", userLoginDto.Username);
        LoginTokenResponseDto authenticationResult = await _userService.LoginUserAsync(userLoginDto);
        _logger.LogInformation("Authentication successful. Session token issued for Username: {Username}", userLoginDto.Username);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,authenticationResult);
    }
}