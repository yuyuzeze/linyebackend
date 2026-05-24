using System.Security.Claims;
using Api.Interfaces;
using Microsoft.Identity.Web;

namespace Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public string? ObjectId =>
        User?.GetObjectId() ??
        User?.FindFirstValue("oid") ??
        User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Upn =>
        User?.FindFirstValue(ClaimTypes.Upn) ??
        User?.FindFirstValue("preferred_username") ??
        User?.FindFirstValue(ClaimTypes.Email);

    public string? DisplayName =>
        User?.FindFirstValue("name") ?? User?.Identity?.Name;
}
