using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController: ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "OK Healthy",
                message = "IssueTracker API is running"
            });
        }
        
    }
}
