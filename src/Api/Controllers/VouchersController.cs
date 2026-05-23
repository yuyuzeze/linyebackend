using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VouchersController : ControllerBase
{
    private readonly IVoucherImportService _importService;
    private readonly IBlobStorageService _blobStorage;

    public VouchersController(IVoucherImportService importService, IBlobStorageService blobStorage)
    {
        _importService = importService;
        _blobStorage = blobStorage;
    }

    /// <summary>
    /// 上传 CSV 文件并导入为传票（用于手动/测试导入，防重使用 api-upload 容器名）.
    /// </summary>
    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(VoucherImportResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<VoucherImportResult>> Import(IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("请上传 CSV 文件");

        var blobName = $"api-upload-{Guid.NewGuid():N}.csv";
        await using var stream = file.OpenReadStream();
        var result = await _importService.ImportFromCsvAsync(stream, "api-upload", blobName, null, cancellationToken);
        return Ok(result);
    }

    [HttpPost("import-from-blob")]
    public async Task<ActionResult<VoucherImportResult>> ImportFromBlob([FromBody] ImportFromBlobRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ContainerName) || string.IsNullOrEmpty(request.BlobName))
            return BadRequest("ContainerName and BlobName are required");

        await using var stream = await _blobStorage.GetContentAsync(request.ContainerName, request.BlobName, cancellationToken);
        var result = await _importService.ImportFromCsvAsync(stream, request.ContainerName, request.BlobName, null, cancellationToken);
        return Ok(result);
    }
}
