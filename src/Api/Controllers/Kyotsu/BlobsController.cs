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
}
