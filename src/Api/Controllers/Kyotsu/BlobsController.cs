using Api.Utility.Storage;
using Api.Utility.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Kyotsu;

[ApiController]
[Route("api/[controller]")]
public class BlobsController : ControllerBase
{
    private readonly IBlobStorageService _blobStorage;
    private readonly IMessageTemplateService _messages;
    private readonly BlobStorageOptions _options;

    public BlobsController(
        IBlobStorageService blobStorage,
        IMessageTemplateService messages,
        IOptions<BlobStorageOptions> options)
    {
        _blobStorage = blobStorage;
        _messages = messages;
        _options = options.Value;
    }

    [HttpGet]
    public async Task<ActionResult<BlobListResult>> List(
        [FromQuery] string? container,
        [FromQuery] string? prefix,
        CancellationToken cancellationToken)
    {
        var containerName = container ?? _options.UploadContainerName;
        var prefixNorm = string.IsNullOrEmpty(prefix) ? _options.UploadPrefix : prefix;
        if (!string.IsNullOrEmpty(prefixNorm) && !prefixNorm.EndsWith("/"))
            prefixNorm += "/";

        try
        {
            var result = await _blobStorage.ListAsync(containerName, prefixNorm, cancellationToken);
            return Ok(result);
        }
        catch (BlobContainerNotFoundException)
        {
            return NotFound(new { error = _messages.Format("MSGXE0101", containerName) });
        }
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download(
        [FromQuery] string path,
        [FromQuery] string? container,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(path))
            return BadRequest(new { error = _messages.Format("MSGXE0104") });

        var containerName = container ?? _options.UploadContainerName;

        try
        {
            var download = await _blobStorage.OpenReadAsync(containerName, path, cancellationToken);
            var fileName = Path.GetFileName(download.BlobName) ?? download.BlobName;
            return File(download.Content, download.ContentType, fileName);
        }
        catch (BlobContainerNotFoundException)
        {
            return NotFound(new { error = _messages.Format("MSGXE0101", containerName) });
        }
        catch (BlobNotFoundException)
        {
            return NotFound(new { error = _messages.Format("MSGXE0102", path) });
        }
    }

    [HttpPost]
    [RequestSizeLimit(52_428_800)]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromQuery] string path,
        [FromQuery] string? container,
        [FromQuery] bool overwrite = true,
        CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
            return BadRequest(new { error = _messages.Format("MSGXE0103") });
        if (string.IsNullOrWhiteSpace(path))
            return BadRequest(new { error = _messages.Format("MSGXE0104") });

        var blobName = path.TrimStart('/');
        var containerName = container ?? _options.UploadContainerName;

        try
        {
            await using var stream = file.OpenReadStream();
            await _blobStorage.UploadAsync(
                containerName,
                blobName,
                stream,
                file.ContentType,
                overwrite,
                cancellationToken);

            return Ok(new { path = blobName, container = containerName, length = file.Length });
        }
        catch (BlobContainerNotFoundException)
        {
            return NotFound(new { error = _messages.Format("MSGXE0101", containerName) });
        }
    }
}
