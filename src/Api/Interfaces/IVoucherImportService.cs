using Api.Models.Dtos;

namespace Api.Interfaces;

public interface IVoucherImportService
{
    Task<VoucherImportResult> ImportFromCsvAsync(
        Stream csvStream,
        string containerName,
        string blobName,
        string? blobETag,
        CancellationToken cancellationToken = default);
}
