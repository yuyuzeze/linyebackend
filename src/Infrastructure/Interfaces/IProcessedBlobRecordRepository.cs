using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IProcessedBlobRecordRepository
{
    Task<ProcessedBlobRecord?> FindByBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<ProcessedBlobRecord> AddAsync(ProcessedBlobRecord entity, CancellationToken cancellationToken = default);
    Task<ProcessedBlobRecord?> UpdateStatusAsync(int id, string status, string? errorMessage, int? rowCount, CancellationToken cancellationToken = default);
    Task AddProcessedRecordWithVouchersAsync(ProcessedBlobRecord record, IEnumerable<Voucher> vouchers, CancellationToken cancellationToken = default);
}
