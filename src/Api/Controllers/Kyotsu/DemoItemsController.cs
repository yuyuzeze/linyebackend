using Api.Controllers;
using Api.Models.Dtos;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Kyotsu;

[ApiController]
[Route("api/[controller]")]
public class DemoItemsController : BaseApiController
{
    private readonly IDemoItemService _service;

    public DemoItemsController(IDemoItemService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DemoItemDto>>>> GetAll(CancellationToken cancellationToken) =>
        FromServiceResult(await _service.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<DemoItemDto>>> GetById(int id, CancellationToken cancellationToken) =>
        FromServiceResult(await _service.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DemoItemDto>>> Create([FromBody] CreateDemoItemDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsSuccess && result.Data is not null)
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, ToResponse(result));
        return FromServiceResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<DemoItemDto>>> Update(int id, [FromBody] UpdateDemoItemDto dto, CancellationToken cancellationToken) =>
        FromServiceResult(await _service.UpdateAsync(id, dto, cancellationToken));

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object?>>> Delete(int id, CancellationToken cancellationToken) =>
        FromServiceResult(await _service.DeleteAsync(id, cancellationToken));

    private static ApiResponse<T> ToResponse<T>(ServiceResult<T> result) =>
        new()
        {
            Result = result.Data,
            Messages = result.Messages,
            StatusDetailMessage = result.StatusDetailMessage,
            StatusCode = result.StatusCode
        };
}
