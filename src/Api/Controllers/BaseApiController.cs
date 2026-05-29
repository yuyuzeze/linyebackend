using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> FromServiceResult<T>(ServiceResult<T> result)
    {
        // 204 No Content は本文不可。ApiResponse エンベロープ付き成功は 200 に正規化する。
        var httpStatus = result is { IsSuccess: true, StatusCode: StatusCodes.Status204NoContent }
            ? StatusCodes.Status200OK
            : result.StatusCode;

        var response = new ApiResponse<T>
        {
            Result = result.IsSuccess ? result.Data : default,
            Messages = result.Messages,
            StatusDetailMessage = result.StatusDetailMessage,
            StatusCode = httpStatus
        };

        return StatusCode(httpStatus, response);
    }
}
