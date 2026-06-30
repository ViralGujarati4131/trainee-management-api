namespace TraineeManagement.Api.CorrelationId;

public interface ICorrelationIdAccessor
{
    string Get();
}

public class CorrelationIdAccessor : ICorrelationIdAccessor
{
    private const string HeaderName = "X-Correlation-ID";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdAccessor(IHttpContextAccessor accessor)
        => _httpContextAccessor = accessor;

    public string Get() =>
        _httpContextAccessor.HttpContext?.Items[HeaderName]?.ToString()
        ?? Guid.NewGuid().ToString();
}