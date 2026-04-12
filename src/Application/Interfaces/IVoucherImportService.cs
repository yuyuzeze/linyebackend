using Application.DTOs;

namespace Application.Interfaces;

public interface IVoucherImportService
{
    Task<VoucherImportResult> ImportFromCsvAsync(
        Stream csvStream,
        string containerName,
        string blobName,
        string? blobETag,
        CancellationToken cancellationToken = default);
}
