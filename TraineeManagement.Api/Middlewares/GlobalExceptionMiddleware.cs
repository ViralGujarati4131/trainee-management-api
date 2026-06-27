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
        string correlationId = context.TraceIdentifier;

        try
        {
            await _next(context);
        }
        catch (BaseApplicationException ex)
        {
            _logger.LogWarning(
                "Application exception caught. CorrelationId={CorrelationId}, ExceptionType={ExceptionType}, Path={Path}, StatusCode={StatusCode}, ErrorCode={ErrorCode}",
                correlationId,
                ex.GetType().Name,
                context.Request.Path,
                ex.Descriptor.HttpStatusCode,
                ex.Descriptor.CustomCode);

            await WriteResponseAsync(context, ex.Descriptor);
        }
        catch (Exception ex)
        {
            if (ex is MySqlException mysqlEx || ex.InnerException is MySqlException { } nestedMysqlEx && (mysqlEx = nestedMysqlEx) != null)
            {
                _logger.LogError(
                    mysqlEx,
                    "Database dependency failure. CorrelationId={CorrelationId}, Path={Path}, MySqlErrorCode={MySqlErrorCode}",
                    correlationId,
                    context.Request.Path,
                    mysqlEx.Number);

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
                _logger.LogError(
                    ex,
                    "Unhandled exception. CorrelationId={CorrelationId}, Path={Path}, Method={Method}",
                    correlationId,
                    context.Request.Path,
                    context.Request.Method);

                await WriteResponseAsync(context, CustomResponse.InternalServerError);
            }
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, CustomResponseDescriptor descriptor)
    {
        context.Response.StatusCode = descriptor.HttpStatusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            Code = descriptor.CustomCode,
            Message = descriptor.Message
        });
    }
}