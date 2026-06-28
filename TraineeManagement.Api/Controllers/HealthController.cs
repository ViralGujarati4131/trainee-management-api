using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.ConstRoute;

namespace TraineeManagement.Api.HealthController;

[ApiController]
[Route(CustomConstRoute.Health)]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetMessage()
    {
        _logger.LogInformation("State check: Processing request.");
        
        return Ok(new
        {
            Status = "running",
            Application = "Trainee Management API",
            Timestamp = DateTime.UtcNow
        });
    }
}