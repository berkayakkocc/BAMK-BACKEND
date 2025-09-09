using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    public class HealthController : BaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            var healthData = new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            };
            
            return SuccessResponse(healthData, "BAMK API is running!");
        }
    }
}
