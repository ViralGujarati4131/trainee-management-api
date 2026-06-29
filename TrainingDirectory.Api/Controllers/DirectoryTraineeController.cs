using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.TraineeDTO;
using TraineeManagement.Api.ResponsesBuilder;
using TrainingDirectory.Api.DirectoryTraineeServiceInterface;
using TraineeManagement.Api.Data.Response;

namespace TrainingDirectory.Api.TraineeControllers;

[ApiController]
[Route(CustomConstRoute.DirectoryTrainee)]
public class TraineeController : ControllerBase
{
    private readonly IDirectoryTraineeService _directoryTraineeService;
    private readonly ILogger<TraineeController> _logger;

    public TraineeController(
        IDirectoryTraineeService directoryTraineeService, 
        ILogger<TraineeController> logger)
    {
        _directoryTraineeService = directoryTraineeService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTraineeProfileByIdAsync(int id, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Processing endpoint request. TraineeId: {TraineeId}", id);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }

        if (id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. TraineeId: {TraineeId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }

        TraineeResponseDto trainee = await _directoryTraineeService.GetTraineeByIdAsync(id, cancellationToken);

        _logger.LogInformation("Endpoint execution completed successfully. TraineeId: {TraineeId}", id);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            trainee
        );
    }
}