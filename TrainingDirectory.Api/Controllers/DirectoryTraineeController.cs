using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.TraineeDTO;
using TraineeManagement.Api.ResponsesBuilder;
using TrainingDirectory.Api.DirectoryTraineeServiceInterface;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.CorrelationId;
using Microsoft.Extensions.Logging;

namespace TrainingDirectory.Api.TraineeControllers;

[ApiController]
[Route(CustomConstRoute.DirectoryTrainee)]
public class TraineeController : ControllerBase
{
    private readonly IDirectoryTraineeService _directoryTraineeService;
    private readonly ILogger<TraineeController> _logger;
    private readonly ICorrelationIdAccessor _correlationIdAccessor;

    public TraineeController(
        IDirectoryTraineeService directoryTraineeService, 
        ILogger<TraineeController> logger,
        ICorrelationIdAccessor correlationIdAccessor)
    {
        _directoryTraineeService = directoryTraineeService;
        _logger = logger;
        _correlationIdAccessor = correlationIdAccessor;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTraineeProfileByIdAsync(int id, CancellationToken cancellationToken)
    {
        string correlationId = _correlationIdAccessor.Get();

        _logger.LogInformation("Processing endpoint request. TraineeId: {TraineeId}, CorrelationId: {CorrelationId}", id, correlationId);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state. CorrelationId: {CorrelationId}", correlationId);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }

        if (id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. TraineeId: {TraineeId}, CorrelationId: {CorrelationId}", id, correlationId);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }

        TraineeResponseDto trainee = await _directoryTraineeService.GetTraineeByIdAsync(id, cancellationToken);

        _logger.LogInformation("Endpoint execution completed successfully. TraineeId: {TraineeId}, CorrelationId: {CorrelationId}", id, correlationId);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            trainee
        );
    }
}