using TraineeManagementApi.Utils.CustomException;
using MySqlConnector;
using TraineeManagementApi.Constants;

namespace TraineeManagementApi.GlobalExceptionMiddleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

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
        catch (BaseApplicationException ex)
        {
            _logger.LogWarning("{ExceptionType} Caught: {Message}", ex.GetType().Name, ex.Message);
            await WriteResponseAsync(context, ex.Descriptor);
        }
        catch (Exception ex)
        {
            if (ex.InnerException is MySqlException mysqlEx)
            {
                _logger.LogError("Database Constraint Violation Encountered. Error Code: {Num}", mysqlEx.Number);
                
                ApiResponseDescriptor dbDescriptor = mysqlEx.Number switch
                {
                    AppConstants.Database.MySqlErrorCodes.NotFoundReference => AppConstants.ApiResponse.SqlReferenceConflict,
                    AppConstants.Database.MySqlErrorCodes.DeleteReference => AppConstants.ApiResponse.SqlDeleteReferenceError,
                    AppConstants.Database.MySqlErrorCodes.UsernameExists => AppConstants.ApiResponse.UsernameExists,
                    _ => AppConstants.ApiResponse.InternalServerError
                };

                await WriteResponseAsync(context, dbDescriptor);
            }
            else
            {
                _logger.LogError(ex, "Unhandled system failure tracking to destination: {Path}", context.Request.Path);
                await WriteResponseAsync(context, AppConstants.ApiResponse.InternalServerError);
            }
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, ApiResponseDescriptor descriptor, object? data = null)
    {
        context.Response.StatusCode = descriptor.HttpStatusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            Code = descriptor.CustomCode,
            Message = descriptor.Message,
            Data = data
        });
    }
}