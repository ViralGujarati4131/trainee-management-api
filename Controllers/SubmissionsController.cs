using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Submissions.DTOs;
using TraineeManagementApi.Submissions.ServiceInterface;
using TraineeManagementApi.ResponsesBuilder;
using TraineeManagementApi.Constants;

namespace TraineeManagementApi.Submissions.Cotroller;

[ApiController]
[Route(AppConstants.Routes.Submissions)]
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
    public async Task<ActionResult<SubmissionResponseDto>> CreateSubmission([FromBody] SubmissionCreateDto createSubmissionDto)
    {
        if(!ModelState.IsValid)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest,AppConstants.Errors.ValidationFailed,ModelState);
        }
        _logger.LogDebug("Invoking submission service to add a new submission");
        SubmissionResponseDto createdSubmission = await _submissionService.CreateSubmissionAsync(createSubmissionDto);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,createdSubmission);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubmissionResponseDto>>> GetSubmissions()
    {
        _logger.LogDebug("Invoking submission service to fetch all submissions");
        IEnumerable<SubmissionResponseDto> submissions = await _submissionService.GetSubmissionsAsync();
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,submissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubmissionResponseDto>> GetSubmissionById(int id)
    {
        _logger.LogDebug("Invoking submission service to retrieve submission for SubmissionId: {SubmissionId}", id);
        SubmissionResponseDto submission = await _submissionService.GetSubmissionByIdAsync(id);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,submission);
    }
}