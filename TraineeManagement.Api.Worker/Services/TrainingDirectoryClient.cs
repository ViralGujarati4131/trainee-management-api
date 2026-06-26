using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TraineeManagement.Api.Contract.TrainingDirectoryContract;

namespace TraineeManagement.Api.Worker.Services;

public class TrainingDirectoryClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TrainingDirectoryClient> _logger;

    public TrainingDirectoryClient(HttpClient httpClient, ILogger<TrainingDirectoryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<TraineeProfileDto?> GetTraineeProfileAsync(int traineeId, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Executing synchronous HTTP GET request for TraineeId={TraineeId}", traineeId);

            [cite_start]// Pass the cancellation token through to follow engineering requirements 
            var response = await _httpClient.GetAsync($"api/directory/trainee/{traineeId}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TraineeProfileDto>(cancellationToken: cancellationToken);
            }

            _logger.LogWarning("TrainingDirectory returned an unsuccessful status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("The HTTP call to TrainingDirectory was aborted because it timed out.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to internal Directory Service safely.");
            return null; // Fallback will catch this null response [cite: 423]
        }
    }
}