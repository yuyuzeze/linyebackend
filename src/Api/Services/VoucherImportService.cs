using Api.Models.Dtos;
using Api.Interfaces;
using Api.Services.Csv;
using CsvHelper;
using CsvHelper.Configuration;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Globalization;
using System.Text;

namespace Api.Services;

public class VoucherImportService : IVoucherImportService
{
    private readonly IProcessedBlobRecordRepository _processedRepo;

    public VoucherImportService(IProcessedBlobRecordRepository processedRepo)
    {
        _processedRepo = processedRepo;
    }

    public async Task<VoucherImportResult> ImportFromCsvAsync(
        Stream csvStream,
        string containerName,
        string blobName,
        string? blobETag,
        CancellationToken cancellationToken = default)
    {
        var existing = await _processedRepo.FindByBlobAsync(containerName, blobName, cancellationToken);
        if (existing is not null && existing.Status == "Completed")
            return new VoucherImportResult(SkippedAlreadyProcessed: true, SuccessCount: 0, FailureCount: 0, ErrorMessage: null);

        var sourceBlobPath = $"{containerName}/{blobName}";
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
            HeaderValidated = null,
        };

        var vouchers = new List<Voucher>();
        int rowIndex = 0;

        try
        {
            using var reader = new StreamReader(csvStream, Encoding.UTF8, leaveOpen: true);
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<VoucherCsvRowMap>();
            await foreach (var row in csv.GetRecordsAsync<VoucherCsvRow>(cancellationToken))
            {
                rowIndex++;
                if (string.IsNullOrWhiteSpace(row.Summary) && string.IsNullOrWhiteSpace(row.Amount))
                    continue;

                if (!DateTime.TryParse(row.VoucherDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var voucherDate))
                    voucherDate = DateTime.UtcNow.Date;

                if (!decimal.TryParse(row.Amount?.Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                    amount = 0;

                vouchers.Add(new Voucher
                {
                    VoucherDate = voucherDate,
                    Summary = (row.Summary ?? "").Trim(),
                    DebitAccount = row.DebitAccount?.Trim(),
                    CreditAccount = row.CreditAccount?.Trim(),
                    Amount = amount,
                    SourceBlobPath = sourceBlobPath,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            return new VoucherImportResult(
                SkippedAlreadyProcessed: false,
                SuccessCount: 0,
                FailureCount: rowIndex,
                ErrorMessage: ex.Message);
        }

        if (vouchers.Count == 0)
        {
            var record = new ProcessedBlobRecord
            {
                ContainerName = containerName,
                BlobName = blobName,
                BlobETag = blobETag,
                ProcessedAt = DateTime.UtcNow,
                Status = "Completed",
                RowCount = 0
            };
            await _processedRepo.AddAsync(record, cancellationToken);
            return new VoucherImportResult(SkippedAlreadyProcessed: false, SuccessCount: 0, FailureCount: 0, ErrorMessage: null);
        }

        var processedRecord = new ProcessedBlobRecord
        {
            ContainerName = containerName,
            BlobName = blobName,
            BlobETag = blobETag,
            ProcessedAt = DateTime.UtcNow,
            Status = "Completed",
            RowCount = vouchers.Count
        };

        try
        {
            await _processedRepo.AddProcessedRecordWithVouchersAsync(processedRecord, vouchers, cancellationToken);
        }
        catch (Exception ex)
        {
            return new VoucherImportResult(
                SkippedAlreadyProcessed: false,
                SuccessCount: 0,
                FailureCount: vouchers.Count,
                ErrorMessage: ex.Message);
        }

        return new VoucherImportResult(
            SkippedAlreadyProcessed: false,
            SuccessCount: vouchers.Count,
            FailureCount: 0,
            ErrorMessage: null);
    }
}
