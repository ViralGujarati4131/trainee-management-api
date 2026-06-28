using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.Data.UserDTO;
using TraineeManagement.Api.Data.UserModel;
using TraineeManagement.Api.UserServiceInterface;
using TraineeManagement.Api.JwtService;
using TraineeManagement.Api.Data.DatabaseContext;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.ResponsesBuilder;

namespace TraineeManagement.Api.UserService;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    private readonly IJwtService _jwtService;
    
    private readonly ILogger<UserService> _logger;

    public UserService(AppDbContext context, IJwtService jwtService, ILogger<UserService> logger)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
    }

    private PasswordVerificationResult VerifyPassword(User user, string password, string hashedPassword)
    {
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        return passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
    }

    private LoginTokenResponseDto MapToLoginResponseDto(string token, int expiresInSeconds, User user)
    {
        _logger.LogInformation("Creating response mapping. UserId: {UserId}", user.Id);
        
        return new LoginTokenResponseDto
        (
            token,
            expiresInSeconds,
            new UserResponseDto
            (
                user.Id,
                user.Username,
                user.Role
            )
        );
    }

    private async Task<User> FetchUserByUsernameInternalAsync(string username)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            _logger.LogWarning("Identity missing. Target record not located in store.");
            throw new NotFoundException(CustomResponse.NotFound,"User");
        }
        return user;
    }

    public async Task<LoginTokenResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
    {
        try
        {
            _logger.LogDebug("Evaluating authentication database context verification lookups.");

            User user = await FetchUserByUsernameInternalAsync(userLoginDto.Username);

            _logger.LogDebug("Evaluating verification criteria against matching database record hashes.");
            PasswordVerificationResult verificationResult = VerifyPassword(user, userLoginDto.Password, user.PasswordHash);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Dependency failure: Secure comparison failed due to invalid credential match. UserId: {UserId}", user.Id);

                throw new UnauthorizedException(CustomResponse.Unauthorized);
            }

            _logger.LogInformation("Credentials matched. Initiating security token factory workflows. UserId: {UserId}", user.Id);
            
            string token = _jwtService.GenerateJwtToken(user, out int expiryMinutes);
            int expiresInSeconds = expiryMinutes * 60;
            
            _logger.LogInformation("Security token generation completed. DurationMinutes: {Duration}", expiryMinutes);
            
            return MapToLoginResponseDto(token, expiresInSeconds, user);
        }
        catch (NotFoundException)
        {
            _logger.LogWarning("Dependency failure: Secure comparison failed due to unmatched identifier key sequences.");
            
            throw new UnauthorizedException(CustomResponse.Unauthorized);
        }
    }
}