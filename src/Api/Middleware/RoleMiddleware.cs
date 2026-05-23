using System.Security.Claims;
using Application.Interfaces;
using Microsoft.Identity.Web;

namespace Api.Middleware;

/// <summary>
/// 認証済みユーザの業務ロールを DB から読み込み、未割当なら 403。
/// </summary>
public class RoleMiddleware
{
    private static readonly PathString[] ExcludedPrefixes =
    [
        new("/health"),
        new("/swagger"),
        new("/api/auth")
    ];

    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public RoleMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context, IUserRoleRepository userRoleRepository)
    {
        if (!_configuration.GetValue("Authentication:Enabled", false))
        {
            await _next(context);
            return;
        }

        if (IsExcluded(context.Request.Path))
        {
            await _next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        var oid = context.User.GetObjectId()
            ?? context.User.FindFirstValue("oid")
            ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(oid))
        {
            await WriteForbiddenAsync(context, "Missing user object id in token.");
            return;
        }

        var userRoles = await userRoleRepository.GetActiveByObjectIdAsync(oid, context.RequestAborted);
        if (userRoles.Count == 0)
        {
            await WriteForbiddenAsync(context, "User has no application role assigned.");
            return;
        }

        if (context.User.Identity is ClaimsIdentity identity)
        {
            var existing = identity.FindAll(ClaimTypes.Role).Select(c => c.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var role in userRoles.Select(r => r.RoleCode).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!existing.Contains(role))
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }

        await _next(context);
    }

    private static bool IsExcluded(PathString path)
    {
        foreach (var prefix in ExcludedPrefixes)
        {
            if (path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private static async Task WriteForbiddenAsync(HttpContext context, string message)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = message });
    }
}
