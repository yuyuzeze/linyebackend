using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> FromServiceResult<T>(ServiceResult<T> result)
    {
        var response = new ApiResponse<T>
        {
            Result = result.IsSuccess ? result.Data : default,
            Messages = result.Messages,
            StatusDetailMessage = result.StatusDetailMessage,
            StatusCode = result.StatusCode
        };

        return StatusCode(result.StatusCode, response);
    }
}
