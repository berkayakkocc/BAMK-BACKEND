using BAMK.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BAMK.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected IActionResult SuccessResponse<T>(T data, string message = "İşlem başarılı")
        {
            return Ok(new ApiResponse<T>(data, message, true));
        }

        protected IActionResult SuccessResponse(string message = "İşlem başarılı")
        {
            return Ok(new ApiResponse(message, true));
        }

        protected IActionResult ErrorResponse(string message, int statusCode = 400, object? errors = null)
        {
            var response = new ApiResponse(message, false)
            {
                Errors = errors
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
            var hasPreviousPage = page > 1;
            var hasNextPage = page < totalPages;

            return Ok(new
            {
                data = data,
                pagination = new
                {
                    page = page,
                    limit = limit,
                    total = total,
                    totalPages = totalPages,
                    hasPreviousPage = hasPreviousPage,
                    hasNextPage = hasNextPage
                },
                message = message,
                success = true,
                timestamp = DateTime.UtcNow
            });
        }

        protected IActionResult HandleResult<T>(Result<T> result, string successMessage = "İşlem başarılı", string errorMessage = "İşlem başarısız")
        {
            if (!result.IsSuccess)
            {
                return ErrorResponse(errorMessage, GetStatusCode(result.Error?.Code ?? ErrorCode.InvalidOperation), result.Error?.Message);
            }

            return SuccessResponse(result.Value, successMessage);
        }

        protected IActionResult HandleResult(Result result, string successMessage = "İşlem başarılı", string errorMessage = "İşlem başarısız")
        {
            if (!result.IsSuccess)
            {
                return ErrorResponse(errorMessage, GetStatusCode(result.Error?.Code ?? ErrorCode.InvalidOperation), result.Error?.Message);
            }

            return SuccessResponse(successMessage);
        }

        protected IActionResult HandlePagedResult<T>(Result<PagedResult<T>> result, string successMessage = "İşlem başarılı", string errorMessage = "İşlem başarısız")
        {
            if (!result.IsSuccess)
            {
                return ErrorResponse(errorMessage, GetStatusCode(result.Error?.Code ?? ErrorCode.InvalidOperation), result.Error?.Message);
            }

            var pagedResult = result.Value;
            return PaginatedResponse(pagedResult.Items, pagedResult.Page, pagedResult.PageSize, pagedResult.TotalCount, successMessage);
        }

        private static int GetStatusCode(ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFound => 404,
                ErrorCode.ValidationError => 400,
                ErrorCode.Unauthorized => 401,
                ErrorCode.Forbidden => 403,
                ErrorCode.Conflict => 409,
                ErrorCode.InternalServerError => 500,
                _ => 400
            };
        }
    }
}
