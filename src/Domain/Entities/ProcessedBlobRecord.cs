namespace Domain.Entities;

public class ProcessedBlobRecord
{
    public int Id { get; set; }
    public string ContainerName { get; set; } = string.Empty;
    public string BlobName { get; set; } = string.Empty;
    public string? BlobETag { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string Status { get; set; } = "Completed"; // Completed | Failed
    public string? ErrorMessage { get; set; }
    public int? RowCount { get; set; }
}
