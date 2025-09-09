using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult SuccessResponse<T>(T data, string message = "İşlem başarılı")
        {
            return Ok(new
            {
                data = data,
                message = message,
                success = true
            });
        }

        protected IActionResult SuccessResponse(string message = "İşlem başarılı")
        {
            return Ok(new
            {
                data = (object?)null,
                message = message,
                success = true
            });
        }

        protected IActionResult ErrorResponse(string message, int statusCode = 400, object? errors = null)
        {
            var response = new
            {
                message = message,
                success = false,
                errors = errors
            };

            return statusCode switch
            {
                400 => BadRequest(response),
                401 => Unauthorized(response),
                403 => Forbid(),
                404 => NotFound(response),
                422 => UnprocessableEntity(response),
                _ => StatusCode(statusCode, response)
            };
        }

        protected IActionResult PaginatedResponse<T>(IEnumerable<T> data, int page, int limit, int total, string message = "İşlem başarılı")
        {
            var totalPages = (int)Math.Ceiling((double)total / limit);
            
            return Ok(new
            {
                data = data,
                pagination = new
                {
                    page = page,
                    limit = limit,
                    total = total,
                    totalPages = totalPages
                },
                message = message,
                success = true
            });
        }
    }
}
