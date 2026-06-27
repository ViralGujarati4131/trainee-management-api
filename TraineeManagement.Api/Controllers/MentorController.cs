using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.MentorDTO;
using TraineeManagement.Api.MentorServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.MentorController;

[ApiController]
[Route(CustomConstRoute.Mentors)]
[Authorize]
public class MentorController : ControllerBase
{
    private readonly IMentorServices _mentorService;
    
    private readonly ILogger<MentorController> _logger;

    public MentorController(IMentorServices mentorService, ILogger<MentorController> logger)
    {
        _mentorService = mentorService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetMentors()
    {
        _logger.LogDebug("Invoking mentor service to fetch all mentors");

        IEnumerable<MentorResponseDto> mentors = await _mentorService.GetMentorsAsync();

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            mentors
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetMentorById(int id)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking mentor service to retrieve profile for MentorId: {MentorId}", id);

        MentorResponseDto mentor = await _mentorService.GetMentorByIdAsync(id);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            mentor
        );
    }

    [HttpPost]
    public async Task<ActionResult> CreateMentor([FromBody] MentorCreateDto createMentorDto)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        _logger.LogDebug("Invoking mentor service to establish a new mentor registration");

        MentorResponseDto createdMentor = await _mentorService.CreateMentorAsync(createMentorDto);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataInsertedSuccess,
            createdMentor
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMentorById(int id)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking mentor service to delete record for MentorId: {MentorId}", id);

        await _mentorService.DeleteMentorByIdAsync(id);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataDeletedNoContent
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMentorById(int id, [FromBody] MentorUpdateDto updateMentorDto)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking mentor service to modify records for MentorId: {MentorId}", id);

        MentorResponseDto updatedMentor = await _mentorService.UpdateMentorByIdAsync(id, updateMentorDto);
        
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataUpdatedSuccess,
            updatedMentor
        );
    }
}