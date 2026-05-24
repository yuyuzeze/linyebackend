using Api.Interfaces;
using Serilog.Context;

namespace Api.Middleware;

/// <summary>
/// RequestId / UPN を Serilog LogContext に注入し、Controller・Service の ILogger で利用する。
/// </summary>
public class RequestLogContextMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLogContextMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUser)
    {
        using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
        using (LogContext.PushProperty("Upn", currentUser.Upn ?? string.Empty))
        {
            await _next(context);
        }
    }
}
