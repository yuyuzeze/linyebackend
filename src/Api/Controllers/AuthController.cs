using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ICurrentUserService _currentUser;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IConfiguration _configuration;

    public AuthController(
        ICurrentUserService currentUser,
        IUserRoleRepository userRoleRepository,
        IConfiguration configuration)
    {
        _currentUser = currentUser;
        _userRoleRepository = userRoleRepository;
        _configuration = configuration;
    }

    /// <summary>当前用户身份与 DB 业务角色（认证关闭时返回开发用模拟用户）。</summary>
    [HttpGet("me")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthMeDto>> Me(CancellationToken cancellationToken)
    {
        var authEnabled = _configuration.GetValue("Authentication:Enabled", false);

        if (!authEnabled)
            return Ok(BuildDevUser(authEnabled));

        if (!_currentUser.IsAuthenticated)
            return Unauthorized();

        var oid = _currentUser.ObjectId;
        if (string.IsNullOrEmpty(oid))
            return Unauthorized();

        var userRoles = await _userRoleRepository.GetActiveByObjectIdAsync(oid, cancellationToken);
        var firstDept = userRoles.Select(r => r.Department).FirstOrDefault(d => d != null);

        return Ok(new AuthMeDto
        {
            ObjectId = oid,
            Upn = _currentUser.Upn ?? string.Empty,
            DisplayName = _currentUser.DisplayName,
            DepartmentCode = firstDept?.Code,
            DepartmentName = firstDept?.Name,
            Roles = userRoles.Select(r => r.RoleCode).Distinct(StringComparer.OrdinalIgnoreCase).ToList(),
            AuthEnabled = true
        });
    }

    private AuthMeDto BuildDevUser(bool authEnabled) =>
        new()
        {
            ObjectId = _configuration["Authentication:DevUser:ObjectId"] ?? "00000000-0000-0000-0000-000000000001",
            Upn = _configuration["Authentication:DevUser:Upn"] ?? "dev@local.test",
            DisplayName = _configuration["Authentication:DevUser:DisplayName"] ?? "本地开发用户",
            DepartmentCode = "DEV",
            DepartmentName = "開発部門",
            Roles = ["All"],
            AuthEnabled = authEnabled
        };
}
