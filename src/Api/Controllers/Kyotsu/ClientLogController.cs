using Api.Models.Dtos;
using Api.Utility.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Kyotsu;

[ApiController]
[Route("api/client-log")]
[AllowAnonymous]
public class ClientLogController : ControllerBase
{
    private readonly ILogger<ClientLogController> _logger;
    private readonly IValidator<ClientLogEntryDto> _validator;

    public ClientLogController(
        ILogger<ClientLogController> logger,
        IValidator<ClientLogEntryDto> validator)
    {
        _logger = logger;
        _validator = validator;
    }

    [HttpPost]
    public IActionResult Post([FromBody] ClientLogEntryDto dto)
    {
        var validation = _validator.Validate(dto);
        if (!validation.IsValid)
            return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

        var messageId = string.IsNullOrWhiteSpace(dto.MessageId)
            ? AppMessageIds.ClientReport
            : dto.MessageId;

        var text = string.IsNullOrWhiteSpace(dto.Url)
            ? dto.Message
            : $"{dto.Message} (URL: {dto.Url})";

        if (!string.IsNullOrWhiteSpace(dto.Stack))
            text = $"{text}{Environment.NewLine}{dto.Stack}";

        if (string.Equals(dto.Level, "Critical", StringComparison.OrdinalIgnoreCase)
            || string.Equals(dto.Level, "Fatal", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogAppCritical(messageId, "クライアント報告: {Message}", text);
        }
        else
        {
            _logger.LogAppError(messageId, "クライアント報告: {Message}", text);
        }

        return NoContent();
    }
}
