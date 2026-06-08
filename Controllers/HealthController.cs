using Microsoft.AspNetCore.Mvc;

namespace TraineeManagement.Api.Controllers;

[ApiController]
// [Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet("api/health")]
    public IActionResult GetMessage()
    {
            return Ok(new {
                status= "running",
                application=  "Trainee Management API", 
                timestamp=  DateTime.Now
                });
    }  
 }
    

