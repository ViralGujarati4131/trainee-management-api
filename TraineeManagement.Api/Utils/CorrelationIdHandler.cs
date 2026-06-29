using TraineeManagement.Api.CorrelationId;

namespace TraineeManagement.Api.CorrelationIdsHandler;

public class CorrelationIdHandler : DelegatingHandler
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly ICorrelationIdAccessor _correlationIdAccessor;

    public CorrelationIdHandler(ICorrelationIdAccessor correlationIdAccessor)
    {
        _correlationIdAccessor = correlationIdAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string correlationId = _correlationIdAccessor.Get();
        request.Headers.TryAddWithoutValidation(CorrelationIdHeader, correlationId);
        return await base.SendAsync(request, cancellationToken);
    }
}