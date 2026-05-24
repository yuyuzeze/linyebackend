using Api.Models.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Kyotsu;

[ApiController]
[Route("api/[controller]")]
public class BlobsController : ControllerBase
{
    private readonly IBlobStorageService _blobStorage;
    private readonly IConfiguration _config;

    public BlobsController(IBlobStorageService blobStorage, IConfiguration config)
    {
        _blobStorage = blobStorage;
        _config = config;
    }

    [HttpGet]
    public async Task<ActionResult<BlobListResultDto>> List(
        [FromQuery] string? container,
        [FromQuery] string? prefix,
        CancellationToken cancellationToken)
    {
        var containerName = container ?? _config["BlobStorage:UploadContainerName"] ?? "csv-inbox";
        var prefixNorm = string.IsNullOrEmpty(prefix) ? _config["BlobStorage:UploadPrefix"] ?? "uploads/" : prefix;
        if (!string.IsNullOrEmpty(prefixNorm) && !prefixNorm.EndsWith("/"))
            prefixNorm += "/";

        try
        {
            var result = await _blobStorage.ListAsync(containerName, prefixNorm, cancellationToken);
            return Ok(result);
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404 || ex.ErrorCode == "ContainerNotFound")
        {
            return NotFound(new { error = "コンテナが見つかりません。" });
        }
    }

    [HttpGet("content")]
    public async Task<IActionResult> GetContent(
        [FromQuery] string? container,
        [FromQuery] string blobName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(blobName))
            return BadRequest("blobName は必須です。");

        var containerName = container ?? _config["BlobStorage:UploadContainerName"] ?? "csv-inbox";

        try
        {
            var stream = await _blobStorage.GetContentAsync(containerName, blobName, cancellationToken);
            return File(stream, "text/plain; charset=utf-8", Path.GetFileName(blobName));
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            return NotFound();
        }
    }
}
