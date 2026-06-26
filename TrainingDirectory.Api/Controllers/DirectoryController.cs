using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Contract.TrainingDirectoryContract;

namespace TrainingDirectory.Api.Controllers;

[ApiController]
[Route("api/directory")]
public class DirectoryController : ControllerBase
{
    [HttpGet("trainee/{id}")]
    public IActionResult GetTraineeProfile(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid Trainee ID allocation.");
        }

        [cite_start]// Return a read-only data profile mockup to represent service communication 
        var profile = new TraineeProfileDto
        {
            TraineeId = id,
            FullName = $"Trainee Jarvis {id}",
            Email = $"trainee.{id}@training.com",
            CurrentTechStack = ".NET, Redis, RabbitMQ",
            TrainingStatus = "Active"
        };

        return Ok(profile);
    }
}