using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.SubmissionDTO;
using TraineeManagement.Api.SubmissionServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.SubmissionController; 

[ApiController]
[Route(CustomConstRoute.Submissions)]
[Authorize]
public class SubmissionsController : ControllerBase
{
    private readonly ISubmissionService _submissionService;

    private readonly ILogger<SubmissionsController> _logger;

    public SubmissionsController(ISubmissionService submissionService, ILogger<SubmissionsController> logger)
    {
        _submissionService = submissionService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> CreateSubmission([FromBody] SubmissionCreateDto createSubmissionDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        _logger.LogDebug("Invoking submission service to add a new submission");

        SubmissionResponseDto createdSubmission = await _submissionService.CreateSubmissionAsync(createSubmissionDto);

        _logger.LogInformation("State check: Submission creation success. Id: {SubmissionId}", createdSubmission.Id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataInsertedSuccess,
            createdSubmission
        );
    }

    [HttpGet]
    public async Task<ActionResult> GetSubmissions()
    {
        _logger.LogDebug("Invoking submission service to fetch all submissions");

        IEnumerable<SubmissionResponseDto> submissions = await _submissionService.GetSubmissionsAsync();

        _logger.LogInformation("State check: Bulk fetch submissions success.");
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            submissions
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetSubmissionById(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {SubmissionId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking submission service to retrieve submission for SubmissionId: {SubmissionId}", id);

        SubmissionResponseDto submission = await _submissionService.GetSubmissionByIdAsync(id);

        _logger.LogInformation("State check: Fetch submission by ID success. Id: {SubmissionId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            submission
        );
    }
}