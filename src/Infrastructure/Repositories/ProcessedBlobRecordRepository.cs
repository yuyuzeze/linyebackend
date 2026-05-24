using Infrastructure.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProcessedBlobRecordRepository : IProcessedBlobRecordRepository
{
    private readonly AppDbContext _db;

    public ProcessedBlobRecordRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProcessedBlobRecord?> FindByBlobAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        return await _db.ProcessedBlobRecords
            .FirstOrDefaultAsync(x => x.ContainerName == containerName && x.BlobName == blobName, cancellationToken);
    }

    public async Task<ProcessedBlobRecord> AddAsync(ProcessedBlobRecord entity, CancellationToken cancellationToken = default)
    {
        _db.ProcessedBlobRecords.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<ProcessedBlobRecord?> UpdateStatusAsync(int id, string status, string? errorMessage, int? rowCount, CancellationToken cancellationToken = default)
    {
        var record = await _db.ProcessedBlobRecords.FindAsync(new object[] { id }, cancellationToken);
        if (record is null) return null;
        record.Status = status;
        record.ErrorMessage = errorMessage;
        record.RowCount = rowCount;
        await _db.SaveChangesAsync(cancellationToken);
        return record;
    }

    public async Task AddProcessedRecordWithVouchersAsync(ProcessedBlobRecord record, IEnumerable<Voucher> vouchers, CancellationToken cancellationToken = default)
    {
        _db.Vouchers.AddRange(vouchers);
        _db.ProcessedBlobRecords.Add(record);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
