using TraineeManagement.Api.Data.CustomException;
using MySqlConnector;
using TraineeManagement.Api.Data.ResponseDescriptor;
using TraineeManagement.Api.Data.Constants;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.GlobalExceptionMiddleware;

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
                
                CustomResponseDescriptor dbDescriptor = mysqlEx.Number switch
                {
                    AppConstants.Database.MySqlErrorCodes.NotFoundReference => CustomResponse.SqlReferenceConflict,
                    AppConstants.Database.MySqlErrorCodes.DeleteReference => CustomResponse.SqlDeleteReferenceError,
                    AppConstants.Database.MySqlErrorCodes.UsernameExists => CustomResponse.UsernameExists,
                    _ => CustomResponse.InternalServerError
                };

                await WriteResponseAsync(context, dbDescriptor);
            }
            else
            {
                _logger.LogError(ex, "Unhandled system failure tracking to destination: {Path}", context.Request.Path);
                await WriteResponseAsync(context, CustomResponse.InternalServerError);
            }
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, CustomResponseDescriptor descriptor, object? data = null)
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