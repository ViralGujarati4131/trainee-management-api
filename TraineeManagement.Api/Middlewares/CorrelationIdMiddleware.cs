namespace TraineeManagement.Api.CorrelationIdMiddleware;

public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        string correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault() 
                               ?? Guid.NewGuid().ToString();

        context.Items[CorrelationIdHeader] = CorrelationIdHeader;
        context.Response.Headers[CorrelationIdHeader] = correlationId;

        await _next(context);
    }
}