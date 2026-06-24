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
            return CustomResponseBuilder.CreateValidationErrorResponse();
        }
        _logger.LogDebug("Invoking submission service to add a new submission");

        SubmissionResponseDto createdSubmission = await _submissionService.CreateSubmissionAsync(createSubmissionDto);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Created,
            createdSubmission
        );
    }

    [HttpGet]
    public async Task<ActionResult> GetSubmissions()
    {
        _logger.LogDebug("Invoking submission service to fetch all submissions");

        IEnumerable<SubmissionResponseDto> submissions = await _submissionService.GetSubmissionsAsync();

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Success,
            submissions
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetSubmissionById(int id)
    {
        if (!ModelState.IsValid || id < 1)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse();
        }
        _logger.LogDebug("Invoking submission service to retrieve submission for SubmissionId: {SubmissionId}", id);

        SubmissionResponseDto submission = await _submissionService.GetSubmissionByIdAsync(id);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Success,
            submission
        );
    }
}