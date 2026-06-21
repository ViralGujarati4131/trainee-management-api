using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Constants;

namespace TraineeManagementApi.Health.Controller;

[ApiController]
[Route(AppConstants.Routes.Health)]
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
        _logger.LogInformation("System health status requested.");
        
        return Ok(new
        {
            Status = "running",
            Application = "Trainee Management API",
            Timestamp = DateTime.UtcNow
        });
    }
}