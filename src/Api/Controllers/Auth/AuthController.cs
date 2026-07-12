using Api.Models.Dtos;
using Api.Utility.Logging;
using Api.Interfaces;
using Infrastructure.DataAccess;
using Infrastructure.Queries.Kyotsu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ICurrentUserService _currentUser;
    private readonly IQueryGateway _queries;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ICurrentUserService currentUser,
        IQueryGateway queries,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _currentUser = currentUser;
        _queries = queries;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet("me")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthMeDto>> Me(CancellationToken cancellationToken)
    {
        var authEnabled = _configuration.GetValue("Authentication:Enabled", false);

        if (!authEnabled)
        {
            _logger.LogAppInformation(AppMessageIds.Information, "認証無効モードでユーザー情報を返却しました。");
            return Ok(BuildDevUser(authEnabled));
        }

        if (!_currentUser.IsAuthenticated)
            return Unauthorized();

        var oid = _currentUser.ObjectId;
        if (string.IsNullOrEmpty(oid))
            return Unauthorized();

        var userRoles = await _queries.QueryAsync<ActiveUserRoleRow>(
            nameof(KyotsuQueries.KYOTSU_Q003),
            KyotsuQueries.KYOTSU_Q003,
            new { EntraObjectId = oid },
            cancellationToken);

        var firstDept = userRoles.FirstOrDefault(r => r.DepartmentCode is not null);

        _logger.LogAppInformation(
            AppMessageIds.Information,
            "ユーザー情報を返却しました。UPN={Upn} Roles={RoleCount}",
            _currentUser.Upn ?? string.Empty,
            userRoles.Count);

        return Ok(new AuthMeDto
        {
            ObjectId = oid,
            Upn = _currentUser.Upn ?? string.Empty,
            DisplayName = _currentUser.DisplayName,
            DepartmentCode = firstDept?.DepartmentCode,
            DepartmentName = firstDept?.DepartmentName,
            Roles = userRoles.Select(r => r.RoleCode).Distinct(StringComparer.OrdinalIgnoreCase).ToList(),
            AuthEnabled = true
        });
    }

    private AuthMeDto BuildDevUser(bool authEnabled) =>
        new()
        {
            ObjectId = _configuration["Authentication:DevUser:ObjectId"] ?? "00000000-0000-0000-0000-000000000001",
            Upn = _configuration["Authentication:DevUser:Upn"] ?? "dev@local.test",
            DisplayName = _configuration["Authentication:DevUser:DisplayName"] ?? "ローカル開発ユーザー",
            DepartmentCode = "DEV",
            DepartmentName = "開発部門",
            Roles = ["All"],
            AuthEnabled = authEnabled
        };
}
