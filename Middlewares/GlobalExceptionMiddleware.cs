using TraineeManagementApi.Utils.CustomException;
using MySqlConnector;
namespace TraineeManagementApi.GlobalExceptionMiddleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    const int notFoundReferanceError = 1452;
    const int deleteReferanceError = 1451;
    const int usernameExistsError = 1062;
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Not found: {Message}", ex.Message);
            await WriteResponse(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogWarning("Unauthorized: {Message}", ex.Message);
            await WriteResponse(context, StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Bad request: {Message}", ex.Message);
            await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (JwtOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation: {Message}", ex.Message);
            await WriteResponse(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred while processing authentication please retry");
        }
        catch (Exception ex)
        {
            if (ex.InnerException is MySqlException mysqlEx)
            {
                _logger.LogError($"SQL Exception Occured Error Code : {mysqlEx.Number}");
                if (mysqlEx.Number == notFoundReferanceError)
                {
                    _logger.LogError("Reference operation Error: {ex}", ex);
                    await WriteResponse(context, StatusCodes.Status400BadRequest, "Some of the provided References does conflits ");
                }
                if (mysqlEx.Number == deleteReferanceError)
                {
                    _logger.LogError("Delete operation Referance Error: {ex}", ex);
                    await WriteResponse(context, StatusCodes.Status400BadRequest, "Delete Operation Could not be completed because of existing reference");
                }
                if (mysqlEx.Number == usernameExistsError)
                {
                    _logger?.LogError("Create operation Referance Error: {ex}", ex);
                    await WriteResponse(context, StatusCodes.Status400BadRequest, "Username is already exists");
                }
            }
            else
            {
                _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                    context.Request.Method, context.Request.Path);
                await WriteResponse(context, StatusCodes.Status500InternalServerError, "Something Went Wrong, Please Try Again");
            }
        }
    }

    private static async Task WriteResponse(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { Message = message });
    }
}