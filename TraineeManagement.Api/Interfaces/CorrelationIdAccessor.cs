namespace TraineeManagement.Api.CorrelationId;

public interface ICorrelationIdAccessor
{
    string Get();
}

public class CorrelationIdAccessor : ICorrelationIdAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdAccessor(IHttpContextAccessor accessor) 
        => _httpContextAccessor = accessor;

    public string Get() =>
        _httpContextAccessor.HttpContext?.Items["X-Correlation-ID"]?.ToString() 
        ?? Guid.NewGuid().ToString();
}