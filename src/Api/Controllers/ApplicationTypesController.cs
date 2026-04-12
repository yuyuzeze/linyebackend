using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/application-types")]
public class ApplicationTypesController : ControllerBase
{
    private readonly IApplicationTypeService _typeService;
    private readonly ICsvMappingService _csvMappingService;

    public ApplicationTypesController(IApplicationTypeService typeService, ICsvMappingService csvMappingService)
    {
        _typeService = typeService;
        _csvMappingService = csvMappingService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ApplicationTypeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var list = await _typeService.GetAllTypesAsync(cancellationToken);
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApplicationTypeDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _typeService.GetTypeByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpGet("{id:int}/fields")]
    public async Task<ActionResult<IReadOnlyList<ApplicationTypeFieldDto>>> GetFields(int id, CancellationToken cancellationToken)
    {
        var list = await _typeService.GetFieldsAsync(id, cancellationToken);
        return Ok(list);
    }

    [HttpGet("{id:int}/csv-mappings")]
    public async Task<ActionResult<IReadOnlyList<CsvColumnMappingDto>>> GetCsvMappings(int id, CancellationToken cancellationToken)
    {
        var list = await _typeService.GetCsvMappingsAsync(id, cancellationToken);
        return Ok(list);
    }

    [HttpPost("{id:int}/map-row")]
    public async Task<ActionResult<IReadOnlyDictionary<string, string>>> MapRow(int id, [FromBody] MapCsvRowRequest request, CancellationToken cancellationToken)
    {
        if (request.Values is null) return BadRequest("Values is required");
        var result = await _csvMappingService.MapRowToFieldsAsync(id, request.Values, cancellationToken);
        return Ok(result);
    }
}

public record MapCsvRowRequest(IReadOnlyList<string>? Values);
