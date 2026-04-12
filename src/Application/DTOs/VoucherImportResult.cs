namespace Application.DTOs;

public record ImportFromBlobRequest(string ContainerName, string BlobName);

public record VoucherImportResult(
    bool SkippedAlreadyProcessed,
    int SuccessCount,
    int FailureCount,
    string? ErrorMessage
);
