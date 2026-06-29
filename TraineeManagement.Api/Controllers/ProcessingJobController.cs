using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.DatabaseContext;
using TraineeManagement.Api.Data.ProcessingJobDto;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.ProcessingJobServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using Microsoft.AspNetCore.Authorization;

namespace TraineeManagement.Api.ProcessingJobControllers;

[ApiController]
[Route(CustomConstRoute.ProcessingJob)]
[Authorize]
public class ProcessingJobsController : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly IProcessingJobService _processingJobService;
    private readonly ILogger<ProcessingJobsController> _logger;

    public ProcessingJobsController(AppDbContext context, IProcessingJobService processingJobService, ILogger<ProcessingJobsController> logger)
    {
        _context = context;
        _processingJobService = processingJobService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobStatus(int id)
    {
        _logger.LogInformation("Processing endpoint request. JobId: {JobId}", id);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state. JobId: {JobId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }

        ProcessingJobResponseDto jobTrack = await _processingJobService.GetProcessingJobByIdAsync(id);

        _logger.LogInformation("Job Detail Fetch completed successfully. JobId: {JobId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            jobTrack
        ); 
    }
}