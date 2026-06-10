using Microsoft.AspNetCore.Mvc;
using Users.DTOs;
using Users.Service.Interface;
using Users.Service;

namespace Users.Controllers;

[ApiController]
[Route("api/auth")]

public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService service)
    {
        userService = service;

    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginTokenResponseDto>> LoginUser(UserLoginDto user)
    {
        LoginTokenResponseDto? userAuth = await userService.LoginUserAsync(user);
        if (userAuth == null)
        {
            return Unauthorized();
        }
        return Ok(userAuth);
    }
}