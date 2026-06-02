namespace Api.Utility.Storage;

public class BlobStorageOptions
{
    public const string SectionName = "BlobStorage";

    public string ConnectionString { get; set; } = "";

    public string UploadContainerName { get; set; } = "csv-inbox";

    public string UploadPrefix { get; set; } = "uploads/";
}
