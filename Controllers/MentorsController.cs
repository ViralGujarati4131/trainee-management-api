using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Mentors.DTOs;
using TraineeManagementApi.Mentors.ServiceInterface;

namespace TraineeManagementApi.Mentors.Controller;

[ApiController]
[Route("api/mentors")]
[Authorize]
public class MentorController : ControllerBase
{
    private readonly IMentorServices _mentorService;
    private readonly ILogger<MentorController> _logger;
    public MentorController(IMentorServices mentorService,ILogger<MentorController> logger)
    {
        _mentorService = mentorService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MentorResponseDto>>> GetMentors()
    {
        _logger.LogDebug("Invoking mentor service to fetch all mentors");
        IEnumerable<MentorResponseDto> mentors = await _mentorService.GetMentorsAsync();
        return Ok(mentors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MentorResponseDto>> GetMentorById(int id)
    {
        _logger.LogDebug("Invoking mentor service to retrieve profile for MentorId: {MentorId}", id);
        MentorResponseDto mentor = await _mentorService.GetMentorByIdAsync(id);
        return Ok(mentor);
    }

    [HttpPut]
    public async Task<ActionResult<MentorResponseDto>> CreateMentor([FromBody]MentorCreateDto createMentorDto)
    {
        _logger.LogDebug("Invoking mentor service to establish a new mentor registration");
        MentorResponseDto createdMentor = await _mentorService.CreateMentorAsync(createMentorDto);
        return CreatedAtAction(nameof(GetMentorById), new { id = createdMentor.Id }, createdMentor);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMentorById(int id)
    {
        _logger.LogDebug("Invoking mentor service to delete record for MentorId: {MentorId}", id);
        await _mentorService.DeleteMentorByIdAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MentorResponseDto>> UpdateMentorById(int id,[FromBody]MentorUpdateDto updateMentorDto)
    {
        _logger.LogDebug("Invoking mentor service to modify records for MentorId: {MentorId}", id);
        MentorResponseDto updatedMentor = await _mentorService.UpdateMentorByIdAsync(id,updateMentorDto);
        return Ok(updatedMentor);
    }
}