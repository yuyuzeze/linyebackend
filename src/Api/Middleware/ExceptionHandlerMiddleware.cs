using Api.Utility.Logging;
using System.Net;
using System.Text.Json;

namespace Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogAppCritical(
                AppMessageIds.UnhandledException,
                "レスポンス送信開始済みのため、エラー本文を書き込めません。",
                exception);
            throw exception;
        }

        _logger.LogAppCritical(
            AppMessageIds.UnhandledException,
            "未処理例外が発生しました。",
            exception);

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var detail = _environment.IsDevelopment()
            ? exception.Message
            : "システム管理者にお問い合わせください。";

        var payload = new
        {
            result = (object?)null,
            messages = new
            {
                nrmList = Array.Empty<object>(),
                wrnList = Array.Empty<object>(),
                errList = new[]
                {
                    new { code = AppMessageIds.UnhandledException, message = detail }
                }
            },
            statusdetailmessage = detail,
            statusCode = context.Response.StatusCode
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
