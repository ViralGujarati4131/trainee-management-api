using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Mentors.DTOs;
using TraineeManagementApi.Mentors.ServiceInterface;
using TraineeManagementApi.ResponsesBuilder;
using TraineeManagementApi.Constants;

namespace TraineeManagementApi.Mentors.Controller;

[ApiController]
[Route(AppConstants.Routes.Mentors)]
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
    public async Task<ActionResult<IEnumerable<MentorResponseDto>>> GetMentors()
    {
        _logger.LogDebug("Invoking mentor service to fetch all mentors");
        IEnumerable<MentorResponseDto> mentors = await _mentorService.GetMentorsAsync();
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,mentors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MentorResponseDto>> GetMentorById(int id)
    {
        _logger.LogDebug("Invoking mentor service to retrieve profile for MentorId: {MentorId}", id);
        MentorResponseDto mentor = await _mentorService.GetMentorByIdAsync(id);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,mentor);
    }

    [HttpPost]
    public async Task<ActionResult<MentorResponseDto>> CreateMentor([FromBody] MentorCreateDto createMentorDto)
    {
        if(!ModelState.IsValid)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest,AppConstants.Errors.ValidationFailed,ModelState);
        }
        _logger.LogDebug("Invoking mentor service to establish a new mentor registration");
        MentorResponseDto createdMentor = await _mentorService.CreateMentorAsync(createMentorDto);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,createdMentor);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMentorById(int id)
    {
        _logger.LogDebug("Invoking mentor service to delete record for MentorId: {MentorId}", id);
        await _mentorService.DeleteMentorByIdAsync(id);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status204NoContent);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MentorResponseDto>> UpdateMentorById(int id, [FromBody] MentorUpdateDto updateMentorDto)
    {
        if(!ModelState.IsValid)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest, AppConstants.Errors.ValidationFailed,ModelState);
        }
        _logger.LogDebug("Invoking mentor service to modify records for MentorId: {MentorId}", id);
        MentorResponseDto updatedMentor = await _mentorService.UpdateMentorByIdAsync(id, updateMentorDto);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,updatedMentor);
    }
}